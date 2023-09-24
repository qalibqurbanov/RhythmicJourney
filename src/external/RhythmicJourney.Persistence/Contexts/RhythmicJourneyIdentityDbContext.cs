using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RhythmicJourney.Persistence.Contexts;

public class RhythmicJourneyIdentityDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public RhythmicJourneyIdentityDbContext(DbContextOptions<RhythmicJourneyIdentityDbContext> options) : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry<AppUser>> Result = ChangeTracker.Entries<AppUser>();

        foreach (var data in Result)
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