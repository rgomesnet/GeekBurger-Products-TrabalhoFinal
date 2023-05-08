namespace GeekBurger.Products.Application.AddProduct
{
    public record ItemToUpsert
    {
        public string Name { get; init; } = default!;
    }
}
