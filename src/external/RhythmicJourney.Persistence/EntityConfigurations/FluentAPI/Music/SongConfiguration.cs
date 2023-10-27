using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RhythmicJourney.Persistence.EntityConfigurations.FluentAPI.Music;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder
            .HasKey(song => song.Id);
    }
}