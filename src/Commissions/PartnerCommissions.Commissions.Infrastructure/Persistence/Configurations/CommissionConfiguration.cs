using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Configurations;

public class CommissionConfiguration : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.ToTable("Commissions");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.PartnerExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(c => c.SchemaType)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder.Property(c => c.Amount)
            .HasPrecision(18, 4);

        builder.HasIndex(c => new { c.EventId, c.Level })
            .IsUnique();

        builder.HasIndex(c => c.PartnerExternalId);

        builder.HasIndex(c => c.AccruedAt)
            .HasFilter("\"PaidAt\" IS NULL");
    }
}
