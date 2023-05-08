using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Application.GetProduct
{
    public record ProductToGet
    {
        public Guid StoreId { get; init; }
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;
        public string Image { get; init; } = default!;
        public decimal Price { get; init; } = default!;
        public List<ItemToGet> Items { get; init; } = new();

        public static implicit operator ProductToGet(Product p)
        {
            return new ProductToGet
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Image = p.Image,
                Price = p.Price,
                StoreId = p.Store.StoreId,
                Items = p.Items?.Select(i => new ItemToGet
                {
                    ItemId = i.ItemId,
                    Name = i.Name
                }).ToList() ?? default!
            };
        }
    }
}
