using GeekBurger.Products.Application.GetProduct;
using GeekBurger.Products.Domain.Entities;

namespace GeekBurger.Products.Application
{
    public record ProductChanged
    {
        public ProductState State { get; init; }
        public ProductToGet Product { get; init; } = default!;
    }

    public record ProductChangedMessage
    {
        public ProductState State { get; init; }
        public ProductToGet Product { get; init; } = default!;
    }


}