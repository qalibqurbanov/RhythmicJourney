using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RhythmicJourney.Persistence.Contexts;

public class RhythmicJourneyIdentityDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int> /* int : User ve Role-larin iwledeceyi primary key tipi */
{
    public RhythmicJourneyIdentityDbContext(DbContextOptions<RhythmicJourneyIdentityDbContext> options) : base(options) { }
}