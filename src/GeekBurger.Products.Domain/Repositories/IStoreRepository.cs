using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Domain.Repositories
{
    public interface IStoreRepository
    {
        Task<Store?> GetStoreByName(string storeName);
    }
}
