using GeekBurger.Products.Application.GetProduct;

namespace GeekBurger.Products.Application.AddProduct
{
    public interface IAddProductService
    {
        Task<ProductToGet> AddProduct(ProductToUpsert productToAdd);
    }
}