using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Infrastructure.Persistence.Configurations;

public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.ToTable("WalletTransactions");

        builder.HasKey(t => t.CommissionId);

        builder.Property(t => t.CommissionId)
            .ValueGeneratedNever();

        builder.Property(t => t.UserExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.EventExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasPrecision(18, 4);

        builder.HasIndex(t => new { t.UserExternalId, t.PaidAt });
    }
}
