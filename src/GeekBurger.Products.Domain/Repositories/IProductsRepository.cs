using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task Save();
        bool Add(Product product);
        void Delete(Product product);
        Task<Product?> GetProductById(Guid productId);
        //bool Update(Product product);
        //List<Item> GetFullListOfItems();
        Task<IEnumerable<Product>> GetProductsByStoreName(string storeName);
    }
}
