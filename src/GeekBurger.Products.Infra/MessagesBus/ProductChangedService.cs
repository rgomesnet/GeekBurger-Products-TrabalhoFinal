using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Domain.Repositories;
using GeekBurger.Products.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GeekBurger.Products.Infra.MessagesBus
{
    public class ProductChangedService : IProductChangedService
    {
        private const string Topic = "ProductChangedTopic";

        private Task? _lastTask;
        private CancellationTokenSource _cancelMessages;

        private readonly ILogService _logService;
        private readonly List<ServiceBusMessage> _messages;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;
        private readonly IProductChangedEventRepository _productChangedEventRepository;

        public ProductChangedService(
            ILogService logService,
            IOptions<ServiceBusConfiguration> values,
            IProductChangedEventRepository productChangedEventRepository)
        {
            _logService = logService;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _messages = new List<ServiceBusMessage>();

            _serviceBusConfiguration = values.Value;
            _cancelMessages = new CancellationTokenSource();
            _productChangedEventRepository = productChangedEventRepository;

            EnsureTopicIsCreated()
                .GetAwaiter()
                .GetResult();
        }

        public void AddToMessageList(IEnumerable<EntityEntry<Product>> changes)
        {
            _messages.AddRange((IEnumerable<ServiceBusMessage>)
                changes.Where(entity => entity.State != EntityState.Detached &&
                                        entity.State != EntityState.Unchanged)
                       .Select(GetMessage)
                       .ToList());
        }

        public async void SendMessagesAsync()
        {
            if (_lastTask is not null &&
                !_lastTask.IsCompleted)
            {
                return;
            }

            var client = new ServiceBusClient(_serviceBusConfiguration.ConnectionString);

            _logService.SendMessagesAsync("Product was changed");

            var topicSender = client.CreateSender(Topic);
            _lastTask = SendAsync(topicSender, _cancelMessages.Token);

            await _lastTask;

            var closeTask = topicSender.CloseAsync();
            await closeTask;
            HandleException(closeTask);
        }

        public bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0)
            {
                return true;
            }

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is Microsoft.Azure.ServiceBus.ServiceBusException)
                {
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
                }
            });

            return false;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await EnsureTopicIsCreated();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cancelMessages.Cancel();

            await Task.CompletedTask;
        }

        private void AddOrUpdateEvent(ProductChangedEvent productChangedEvent)
        {
            try
            {
                var evt = _productChangedEventRepository.Get(productChangedEvent.EventId)
                                                        .GetAwaiter()
                                                        .GetResult();

                if (productChangedEvent.EventId == Guid.Empty || (evt is null))
                {
                    _productChangedEventRepository.Add(productChangedEvent);
                }
                else
                {
                    evt.MessageSent = true;
                    _productChangedEventRepository.Update(evt);
                }

                _productChangedEventRepository.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private ServiceBusMessage GetMessage(EntityEntry<Product> entity)
        {
            ProductChangedMessage productChanged = entity;

            var productChangedSerialized =
                JsonSerializer.Serialize(productChanged, _jsonSerializerOptions);

            var productChangedBinaryData =
                new BinaryData(Encoding.UTF8.GetBytes(productChangedSerialized));

            ProductChangedEvent productChangedEvent = entity;
            AddOrUpdateEvent(productChangedEvent);

            return new ServiceBusMessage()
            {
                Body = productChangedBinaryData,
                MessageId = productChangedEvent.EventId.ToString(),
                Subject = productChanged.Product.StoreId.ToString(),

            };
        }

        private async Task SendAsync(ServiceBusSender topicSender, CancellationToken cancellationToken)
        {
            var tries = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_messages.Count <= 0)
                    break;

                ServiceBusMessage? message;
                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }

                var sendTask = topicSender.SendMessageAsync(message, cancellationToken);
                await sendTask;
                var success = HandleException(sendTask);

                if (!success)
                {
                    var cancelled =
                        cancellationToken.WaitHandle
                                         .WaitOne(10000 * (tries < 60 ? tries++ : tries));

                    if (cancelled)
                    {
                        break;
                    }
                }
                else
                {
                    if (message is null)
                    {
                        continue;
                    }

                    AddOrUpdateEvent(new ProductChangedEvent
                    {
                        EventId = new Guid(message.MessageId)
                    });

                    _messages.Remove(message);
                }
            }
        }

        private async Task EnsureTopicIsCreated()
        {
            var client =
                new ServiceBusAdministrationClient(_serviceBusConfiguration.ConnectionString);

            if (!await client.TopicExistsAsync(Topic))
            {
                await client.CreateTopicAsync(Topic);
            }
        }
    }
}
