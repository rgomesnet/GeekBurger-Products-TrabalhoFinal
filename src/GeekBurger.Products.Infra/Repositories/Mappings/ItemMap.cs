using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeekBurger.Products.Infra.Repositories.Mappings
{
    internal class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");

            builder.HasKey(_ => _.ItemId);

            builder.Property(_ => _.ItemId)
                .HasColumnType<Guid>("uniqueidentifier");

            builder.Property(_ => _.Name).HasMaxLength(500);

            builder.HasOne(_ => _.Product)
                   .WithMany(_ => _.Items)
                   .HasForeignKey("ProductId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
