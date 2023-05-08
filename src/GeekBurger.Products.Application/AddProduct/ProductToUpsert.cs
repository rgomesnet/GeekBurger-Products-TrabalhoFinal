using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Application.AddProduct
{
    public record ProductToUpsert
    {
        public string Name { get; init; } = default!;
        public string Image { get; init; } = default!;
        public decimal Price { get; init; } = default!;
        public string StoreName { get; init; } = default!;
        public IEnumerable<ItemToUpsert> Items { get; init; } = Enumerable.Empty<ItemToUpsert>();

        public static implicit operator Product(ProductToUpsert p)
        {
            return new Product
            {
                Image = p.Image,
                Price = p.Price,
                Name = p.Name,
                Store = new Store
                {
                    Name = p.StoreName
                },
                Items = p.Items?.Select(i => new Item
                {
                    Name = i.Name
                }).ToList() ?? default!
            };
        }

        public static implicit operator ProductToUpsert(Product p)
        {
            return new Product
            {
                Image = p.Image,
                Price = p.Price,
                Name = p.Name,
                Store = new Store
                {
                    Name = p.Store.Name,
                    StoreId = p.Store.StoreId
                },
                Items = p.Items?.Select(i => new Item
                {
                    Name = i.Name
                }).ToList() ?? default!
            };
        }
    }
}
