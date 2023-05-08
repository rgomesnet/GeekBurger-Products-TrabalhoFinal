using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GeekBurger.Products.Domain.Entities
{
    public record ProductChangedEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();

        public bool MessageSent { get; set; }

        public ProductState State { get; init; }

        public Product Product { get; set; } = default!;

        public static implicit operator ProductChangedEvent(EntityEntry<Product> entry)
        {
            var stateValue = (int)entry.State;

            return new ProductChangedEvent
            {
                State = (ProductState)stateValue,
                Product = new Product
                {
                    ProductId = entry.Entity.ProductId,
                    Image = entry.Entity.Image,
                    Name = entry.Entity.Name,
                    Price = entry.Entity.Price,
                    Store = new Store
                    {
                        StoreId = entry.Entity.Store.StoreId
                    },
                    Items = entry.Entity.Items?.Select(i => new Item
                    {
                        ItemId = i.ItemId,
                        Name = i.Name
                    }).ToList() ?? default!
                }
            };
        }
    }
}
