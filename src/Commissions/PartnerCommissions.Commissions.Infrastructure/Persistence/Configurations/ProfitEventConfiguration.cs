using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Configurations;

public class ProfitEventConfiguration : IEntityTypeConfiguration<ProfitEvent>
{
    public void Configure(EntityTypeBuilder<ProfitEvent> builder)
    {
        builder.ToTable("ProfitEvents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.UserExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.Profit)
            .HasPrecision(18, 2);

        builder.HasIndex(e => e.ExternalId)
            .IsUnique();

        builder.HasIndex(e => new { e.UserExternalId, e.ReceivedAt });

        builder.HasMany(e => e.Commissions)
            .WithOne(c => c.Event)
            .HasForeignKey(c => c.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
