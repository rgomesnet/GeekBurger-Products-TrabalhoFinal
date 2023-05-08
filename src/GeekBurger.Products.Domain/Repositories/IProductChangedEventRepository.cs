using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Domain.Repositories
{
    public interface IProductChangedEventRepository
    {
        Task Save();
        Task<ProductChangedEvent?> Get(Guid eventId);
        bool Add(ProductChangedEvent productChangedEvent);
        bool Update(ProductChangedEvent productChangedEvent);
    }
}
