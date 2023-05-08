using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Infra.Repositories
{
    public class ProductChangedEventRepository : IProductChangedEventRepository
    {
        private readonly ProductsDbContext _dbContext;

        public ProductChangedEventRepository(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductChangedEvent?> Get(Guid eventId)
        {
            return await _dbContext.ProductChangedEvents
                                   .FirstOrDefaultAsync(product => product.EventId == eventId);
        }

        public bool Add(ProductChangedEvent productChangedEvent)
        {
            productChangedEvent.Product =
                _dbContext.Products
                          .First(_ => _.ProductId == productChangedEvent.Product!.ProductId);

            _dbContext.ProductChangedEvents.Add(productChangedEvent);

            return true;
        }

        public bool Update(ProductChangedEvent productChangedEvent)
        {
            return true;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
