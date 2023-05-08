using GeekBurger.Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeekBurger.Products.Infra.Repositories.Mappings
{
    internal class ProductChangedEventMap : IEntityTypeConfiguration<ProductChangedEvent>
    {
        public void Configure(EntityTypeBuilder<ProductChangedEvent> builder)
        {
            builder.ToTable("ProductChangedEvents");

            builder.HasKey(_ => _.EventId);

            builder.Property(_ => _.EventId)
                .HasColumnType<Guid>("uniqueidentifier");

            builder.Property(_ => _.State)
                   .IsRequired();

            builder.HasOne(_ => _.Product);
        }
    }
}
