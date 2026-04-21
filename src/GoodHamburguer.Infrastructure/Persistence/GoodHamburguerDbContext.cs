using GoodHamburguer.Infrastructure.Persistence.Configurations;
using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburguer.Infrastructure.Persistence;

public sealed class GoodHamburguerDbContext : DbContext
{
    public GoodHamburguerDbContext(DbContextOptions<GoodHamburguerDbContext> options)
        : base(options)
    {
    }

    public DbSet<MenuItemEntity> MenuItems => Set<MenuItemEntity>();

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MenuItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
    }
}
