namespace RhythmicJourney.Core.Entities.Base.Common;

/// <summary>
/// Bu tip ozunde 'IdentityUser'-de olmayan, lakin (useri temsil eden 'AppUser'-i) lazimimiz olan deyiwenlerle temin eden base tipdir.
/// </summary>
public interface ICustomEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}