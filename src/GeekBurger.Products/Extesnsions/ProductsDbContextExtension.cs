using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Infra.Repositories;
using Newtonsoft.Json;

namespace GeekBurger.Products.Extesnsions
{
    public static class ProductsDbContextExtension
    {
        public static void Seed(this ProductsDbContext context)
        {
            context.AddRange(
              new List<Store>
              {
                    new Store {
                        Name = "Paulista",
                        StoreId = new Guid("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e")
                    },
                    new Store {
                        Name = "Morumbi",
                        StoreId = new Guid("8d618778-85d7-411e-878b-846a8eef30c0")
                    }
              });

            var productsTxt = File.ReadAllText("products.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(productsTxt);

            products!.ForEach(p =>
            {
                p.Store = context.Stores.FirstOrDefault(s => s.StoreId == p.Store.StoreId);
            });

            context.Products.AddRange(products!);

            context.SaveChanges();
        }
    }
}
