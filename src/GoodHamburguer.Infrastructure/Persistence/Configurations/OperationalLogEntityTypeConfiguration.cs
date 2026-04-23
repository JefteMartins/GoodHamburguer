using GoodHamburguer.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburguer.Infrastructure.Persistence.Configurations;

internal sealed class OperationalLogEntityTypeConfiguration : IEntityTypeConfiguration<OperationalLogEntity>
{
    public void Configure(EntityTypeBuilder<OperationalLogEntity> builder)
    {
        builder.ToTable("operational_logs");

        builder.HasKey(log => log.Id);

        builder.Property(log => log.Id)
            .ValueGeneratedOnAdd();

        builder.Property(log => log.CreatedAtUtc)
            .HasColumnType("datetime(6)");

        builder.Property(log => log.Type)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(log => log.Route)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(log => log.Method)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(log => log.CorrelationId)
            .HasMaxLength(128);

        builder.Property(log => log.Payload)
            .HasColumnType("json")
            .IsRequired();

        builder.Property(log => log.ExceptionType)
            .HasMaxLength(256);

        builder.Property(log => log.ErrorMessage)
            .HasMaxLength(2048);

        builder.HasIndex(log => log.CreatedAtUtc);
        builder.HasIndex(log => log.Type);
    }
}
