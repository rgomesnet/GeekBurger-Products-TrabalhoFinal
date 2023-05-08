using GeekBurger.Products.Application.GetProduct;
using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GeekBurger.Products.Infra.MessagesBus
{
    public class ProductChangedMessage
    {
        public ProductState State { get; init; }
        public ProductToGet Product { get; init; } = default!;

        public static implicit operator ProductChangedMessage(EntityEntry<Product> entry)
        {
            var stateValue = (int)entry.State;

            return new()
            {
                State = (ProductState)stateValue,
                Product = new ProductToGet
                {
                    ProductId = entry.Entity.ProductId,
                    Image = entry.Entity.Image,
                    Name = entry.Entity.Name,
                    Price = entry.Entity.Price,
                    StoreId = entry.Entity.Store.StoreId,
                    Items = entry.Entity.Items?.Select(i => new ItemToGet
                    {
                        ItemId = i.ItemId,
                        Name = i.Name
                    }).ToList() ?? default!
                }
            };
        }
    }
}
