using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RhythmicJourney.Persistence.EntityConfigurations.FluentAPI.Identity;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .ToTable("RefreshTokens")
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(t => t.User)
            .WithMany(t => t.RefreshTokens)
            .HasForeignKey(t => t.UserID);
    }
}