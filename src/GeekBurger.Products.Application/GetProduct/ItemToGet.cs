namespace GeekBurger.Products.Application.GetProduct
{
    public record ItemToGet
    {
        public Guid ItemId { get; init; }
        public string Name { get; init; } = default!;
    }
}
