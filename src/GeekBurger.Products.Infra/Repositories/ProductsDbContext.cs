using GeekBurger.Products.Domain.Entities;
using GeekBurger.Products.Infra.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace GeekBurger.Products.Infra.Repositories
{
    public class ProductsDbContext : DbContext
    {
        private const string _databaseName = "geekburger-products.db";

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemMap).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseInMemoryDatabase(_databaseName);

        public DbSet<Item> Items { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductChangedEvent> ProductChangedEvents { get; set; }
    }
}
