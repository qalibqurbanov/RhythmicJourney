using RhythmicJourney.Core.Entities.Base;

namespace RhythmicJourney.Core.Entities.Identity;

/// <summary>
/// Her hansi bir istifadecini temsil edir.
/// </summary>
public class AppUser : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}