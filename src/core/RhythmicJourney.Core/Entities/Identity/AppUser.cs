using RhythmicJourney.Core.Entities.Base;

namespace RhythmicJourney.Core.Entities.Identity;

public class AppUser : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}