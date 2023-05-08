namespace GeekBurger.Products.Application.DeleteProduct
{
    public interface IDeleteProductService
    {
        Task DeleteProductById(Guid id);
    }
}