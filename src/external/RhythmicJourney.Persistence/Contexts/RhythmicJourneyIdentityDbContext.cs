using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RhythmicJourney.Persistence.EntityConfigurations.FluentAPI.Identity;

namespace RhythmicJourney.Persistence.Contexts;

/// <summary>
/// Identity DB-i ile data mubadilesi etmek ucun iwledeceyimiz DbContext sinifi.
/// </summary>
public class RhythmicJourneyIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public RhythmicJourneyIdentityDbContext(DbContextOptions<RhythmicJourneyIdentityDbContext> options) : base(options) { }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        // Ve yaxud qisaca: \\
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); /* Hazirki Assembly('Persistence' qati) icerisinde 'IEntityTypeConfiguration' interfeysini implement eden sinifleri vermiw olduq. */

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry<AppUser>> trackedUsers = ChangeTracker.Entries<AppUser>();

        foreach (var data in trackedUsers)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow
            };
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}