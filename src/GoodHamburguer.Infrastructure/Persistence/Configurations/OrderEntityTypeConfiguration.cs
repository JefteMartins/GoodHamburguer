using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburguer.Infrastructure.Persistence.Configurations;

internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.CreatedAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(order => order.UpdatedAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(order => order.SandwichName)
            .HasMaxLength(120);

        builder.Property(order => order.SandwichUnitPrice)
            .HasPrecision(10, 2);

        builder.Property(order => order.SideName)
            .HasMaxLength(120);

        builder.Property(order => order.SideUnitPrice)
            .HasPrecision(10, 2);

        builder.Property(order => order.DrinkName)
            .HasMaxLength(120);

        builder.Property(order => order.DrinkUnitPrice)
            .HasPrecision(10, 2);
    }
}
