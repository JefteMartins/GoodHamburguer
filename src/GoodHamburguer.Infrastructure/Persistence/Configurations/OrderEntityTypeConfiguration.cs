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

        builder.Property(order => order.SandwichItemCode)
            .HasMaxLength(64);

        builder.Property(order => order.SideItemCode)
            .HasMaxLength(64);

        builder.Property(order => order.DrinkItemCode)
            .HasMaxLength(64);

        builder.HasOne<MenuItemEntity>()
            .WithMany()
            .HasForeignKey(order => order.SandwichItemCode)
            .HasPrincipalKey(menuItem => menuItem.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MenuItemEntity>()
            .WithMany()
            .HasForeignKey(order => order.SideItemCode)
            .HasPrincipalKey(menuItem => menuItem.Code)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<MenuItemEntity>()
            .WithMany()
            .HasForeignKey(order => order.DrinkItemCode)
            .HasPrincipalKey(menuItem => menuItem.Code)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
