using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Music;

namespace RhythmicJourney.Persistence.Contexts;

public class RhythmicJourneyStandartDbContext : DbContext
{
    public RhythmicJourneyStandartDbContext(DbContextOptions<RhythmicJourneyStandartDbContext> options) : base(options) { }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Category> Categories { get; set; }
}