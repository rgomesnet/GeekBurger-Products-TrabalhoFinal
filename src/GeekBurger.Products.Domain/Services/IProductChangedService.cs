using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;

namespace GeekBurger.Products.Domain.Services
{
    public interface IProductChangedService : IHostedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<Product>> changes);
    }
}
