using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;

namespace RhythmicJourney.Persistence.Contexts;

/// <summary>
/// Umumi melumatlari saxlayan DB ile data mubadilesi etmek ucun iwledeceyimiz DbContext sinifi.
/// </summary>
public class RhythmicJourneyStandartDbContext : DbContext
{
    public RhythmicJourneyStandartDbContext(DbContextOptions<RhythmicJourneyStandartDbContext> options) : base(options) { }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Category> Categories { get; set; }
}