using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburguer.Infrastructure.Persistence.Configurations;

internal sealed class MenuItemEntityTypeConfiguration : IEntityTypeConfiguration<MenuItemEntity>
{
    public void Configure(EntityTypeBuilder<MenuItemEntity> builder)
    {
        builder.ToTable("menu_items");

        builder.HasKey(menuItem => menuItem.Code);

        builder.Property(menuItem => menuItem.Code)
            .HasMaxLength(64);

        builder.Property(menuItem => menuItem.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(menuItem => menuItem.CategoryCode)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(menuItem => menuItem.CategoryDisplayName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(menuItem => menuItem.Price)
            .HasPrecision(10, 2);
    }
}
