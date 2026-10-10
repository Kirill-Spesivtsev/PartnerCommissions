using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerCommissions.Commissions.Domain.Entities;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Configurations;

public class CommissionSettingsConfiguration : IEntityTypeConfiguration<CommissionSettings>
{
    public void Configure(EntityTypeBuilder<CommissionSettings> builder)
    {
        builder.ToTable("CommissionSettings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.CurrentScheme)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder.HasData(new CommissionSettings
        {
            Id = 1,
            CurrentScheme = SchemaType.Linear,
            UpdatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
        });
    }
}
