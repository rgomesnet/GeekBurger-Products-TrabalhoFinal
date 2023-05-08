using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using GeekBurger.Products.Domain.Services;
using Microsoft.Extensions.Options;
using System.Text;

namespace GeekBurger.Products.Infra.MessagesBus
{
    public class LogService : ILogService
    {
        private Task? _lastTask;
        private List<ServiceBusMessage> _messages;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;

        private const string MicroService = "Products";
        private const string Topic = "Log";

        public LogService(IOptions<ServiceBusConfiguration> values)
        {
            _serviceBusConfiguration = values.Value;

            _messages = new List<ServiceBusMessage>();
            EnsureTopicIsCreated()
                .GetAwaiter()
                .GetResult();
        }

        public async void SendMessagesAsync(string message)
        {
            _messages.Add(GetMessage(message));

            if (_lastTask != null && !_lastTask.IsCompleted)
            {
                return;
            }

            var client =
                new ServiceBusClient(_serviceBusConfiguration.ConnectionString);

            var topicSender = client.CreateSender(Topic);
            _lastTask = SendAsync(topicSender);

            await _lastTask;

            var closeTask = topicSender.CloseAsync();
            await closeTask;
            HandleException(closeTask);
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

        private ServiceBusMessage GetMessage(string message)
        {
            var productChangedBinaryData =
                new BinaryData(Encoding.UTF8.GetBytes(message));

            return new ServiceBusMessage
            {
                Body = productChangedBinaryData,
                MessageId = Guid.NewGuid().ToString(),
                Subject = MicroService
            };
        }

        private async Task SendAsync(ServiceBusSender topicClient)
        {
            int tries = 0;
            ServiceBusMessage? message;
            while (true)
            {
                if (_messages.Count <= 0)
                {
                    break;
                }

                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }

                var sendTask = topicClient.SendMessageAsync(message);
                await sendTask;
                var success = HandleException(sendTask);

                if (!success)
                {
                    Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                }
                else
                {
                    _messages.Remove(message!);
                }
            }
        }
        private bool HandleException(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0)
            {
                return true;
            }

            task.Exception.InnerExceptions.ToList().ForEach(innerException =>
            {
                Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details:{innerException.StackTrace} ");

                if (innerException is ServiceBusException)
                {
                    Console.WriteLine("Connection Problem with Host. Internet Connection can be down");
                }
            });

            return false;
        }
    }
}
