using GeekBurger.Products.Domain.Repositories;

namespace GeekBurger.Products.Application.GetProduct
{
    public class GetProductService : IGetProductService
    {
        private readonly IProductsRepository _repository;

        public GetProductService(IProductsRepository repository)
            => _repository = repository;

        public async Task<ProductToGet> GetProductById(Guid id)
        {
            var product = await _repository.GetProductById(id);
            if (product is null)
                throw new ArgumentException();

            return (ProductToGet)product!;
        }

        public async Task<IEnumerable<ProductToGet>> GetProductsByStoreName(string storeName)
        {
            var products = await _repository.GetProductsByStoreName(storeName);

            IEnumerable<ProductToGet> pg = (IEnumerable<ProductToGet>)products;

            return pg;


        }
    }
}
