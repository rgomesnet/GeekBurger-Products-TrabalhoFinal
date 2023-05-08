namespace GeekBurger.Products.Domain.Entities
{
    public record Product
    {
        public Guid ProductId { get; init; } = Guid.NewGuid();
        public decimal Price { get; init; }
        public Store Store { get; set; } = default!;
        public string Name { get; init; } = string.Empty!;
        public string Image { get; init; } = string.Empty;
        public ICollection<Item> Items { get; init; } = default!;
    }
}
