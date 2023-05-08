using GeekBurger.Products.Domain.Repositories;

namespace GeekBurger.Products.Application.DeleteProduct
{
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductsRepository _repository;

        public DeleteProductService(IProductsRepository repository)
            => _repository = repository;

        public async Task DeleteProductById(Guid id)
        {
            var product = await _repository.GetProductById(id);
            if (product is null)
                throw new ArgumentException();

            _repository.Delete(product);
            await _repository.Save();
        }
    }
}
