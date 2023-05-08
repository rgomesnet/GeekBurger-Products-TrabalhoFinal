using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Domain.Repositories;
using GeekBurger.Products.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Infra.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsDbContext _dbContext;
        private readonly IProductChangedService _productChangedService;

        public ProductsRepository(ProductsDbContext dbContext, IProductChangedService productChangedService)
            => (_dbContext, _productChangedService) = (dbContext, productChangedService);

        public async Task<Product?> GetProductById(Guid productId)
        {
            return await _dbContext.Products
                                   .Include(p => p.Store)
                                   .Include(p => p.Items)
                                   .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public bool Add(Product product)
        {
            _dbContext.Products.Add(product);
            return true;
        }

        public void Delete(Product product)
        {
            _dbContext.Products.Remove(product);
        }

        public async Task Save()
        {
            _productChangedService.AddToMessageList(_dbContext.ChangeTracker.Entries<Product>());

            await _dbContext.SaveChangesAsync();

            _productChangedService.SendMessagesAsync();
        }

        //public List<Item> GetFullListOfItems()
        //{
        //    return _dbContext.Items.ToList();
        //}

        //public bool Update(Product product)
        //{
        //    return true;
        //}

        public async Task<IEnumerable<Product>> GetProductsByStoreName(string storeName)
        {
            return await _dbContext.Products
                .Where(product => product.Store.Name.Equals(storeName, StringComparison.InvariantCultureIgnoreCase))
                .Include(product => product.Items)
                .ToListAsync();
        }
    }
}
