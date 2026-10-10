using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.ExternalId)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(u => u.ExternalId)
            .IsUnique();

        builder.HasOne(u => u.Referrer)
            .WithMany(u => u.Referrals)
            .HasForeignKey(u => u.ReferrerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
