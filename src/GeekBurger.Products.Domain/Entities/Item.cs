namespace GeekBurger.Products.Domain.Entities
{
    public record Item
    {
        public Guid ItemId { get; init; }
        public string Name { get; init; } = default!;
        public Product Product { get; init; } = default!;
    }
}