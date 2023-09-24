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

public class RhythmicJourneyIdentityDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int> /* int : User ve Role-larin iwledeceyi primary key tipi */
{
    public RhythmicJourneyIdentityDbContext(DbContextOptions<RhythmicJourneyIdentityDbContext> options) : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        /* Ilk once mudaxile edeceyim entity-ni elde edirem: */
        IEnumerable<EntityEntry<AppUser>> Result = ChangeTracker.Entries<AppUser>();

        foreach (var data in Result)
        {
            /* 'data'-nin 'State'-i esasinda/wertiyle switch-case bloku aciram: */
            _ = data.State switch
            {
                /* Yaxalanan entitynin state-i 'Added'-dirse, yeni, 'data' entity-sine 'Adding' operasiyasi tetbiq edilibse: 'data' entity-sinin 'CreatedDate' propertysinin deyerini deyiw hazirki tarixe: */
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,

                /* Yaxalanan entitynin state-i 'Added'-dirse, yeni, 'data' entity-sine 'Updating' operasiyasi tetbiq edilibse: 'data' entity-sinin 'UpdatedDate' propertysinin deyerini deyiw hazirki tarixe: */
                EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow
            };
        }

        /* Son olaraq ise yarim qalan hazirki save emeliyyatini davam etdirmek meqsedile 'SaveChangesAsync()' metodunu triggerleyirik: */
        return base.SaveChangesAsync(cancellationToken);
    }
}