using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeekBurger.Products.Infra.Repositories.Mappings
{
    internal class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(_ => _.ProductId);

            builder.Property(_ => _.ProductId)
                .HasColumnType<Guid>("uniqueidentifier");

            builder.Property(_ => _.Name)
                   .HasMaxLength(500);

            builder.Property(_ => _.Image)
                   .HasMaxLength(2500);

            builder.Property(_ => _.Price)
                 .HasMaxLength(2500);

            builder.HasOne(_ => _.Store);

            builder.HasMany(e => e.Items)
                   .WithOne(e => e.Product)
                   .HasForeignKey("ProductId")
                   .IsRequired();
        }
    }
}
