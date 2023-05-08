using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeekBurger.Products.Infra.Repositories.Mappings
{
    internal class StoreMap : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");

            builder.Property(_ => _.StoreId)
                .HasColumnType<Guid>("uniqueidentifier");

            builder.HasKey(_ => _.StoreId);

            builder.Property(_ => _.Name)
                   .HasMaxLength(500);
        }
    }
}
