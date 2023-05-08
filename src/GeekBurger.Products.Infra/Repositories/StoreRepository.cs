using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Infra.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private ProductsDbContext _context { get; set; }

        public StoreRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<Store?> GetStoreByName(string storeName)
        {
            return await _context.Stores
                                 .FirstOrDefaultAsync(store =>
                                    store.Name.Equals(storeName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
