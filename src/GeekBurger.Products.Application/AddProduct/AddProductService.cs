using GeekBurger.Products.Application.GetProduct;
using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Domain.Repositories;
using GeekBurger.Products.Domain.Services;

namespace GeekBurger.Products.Application.AddProduct
{
    public class AddProductService : IAddProductService
    {
        private readonly IProductsRepository _repository;
        private readonly IStoreRepository _storeRepository;

        public AddProductService(
            IProductsRepository repository,
            IStoreRepository storeRepository,
            IProductChangedService productChangedService)
        {
            _repository = repository;
            _storeRepository = storeRepository;
        }

        public async Task<ProductToGet> AddProduct(ProductToUpsert productToAdd)
        {
            Product product = productToAdd;

            var store = await _storeRepository.GetStoreByName(product.Store.Name);

            if (store is null)
            {
                throw new ArgumentException("store name is invalid.");
            }

            product.Store = store;

            _repository.Add(product);
            await _repository.Save();

            ProductToGet productToGet = product;

            return productToGet;
        }
    }
}
