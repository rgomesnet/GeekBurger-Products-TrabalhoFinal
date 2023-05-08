namespace GeekBurger.Products.Application.GetProduct
{
    public interface IGetProductService
    {
        Task<ProductToGet> GetProductById(Guid id);

        Task<IEnumerable<ProductToGet>> GetProductsByStoreName(string storeName);
    }
}