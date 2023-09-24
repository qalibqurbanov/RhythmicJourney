namespace RhythmicJourney.Application.Features.Identity.Common;

/// <summary>
/// Clientin sign-up meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datalari temsil edir.
/// </summary>
public record RegisterRequest(string FirstName, string LastName, string Email, string Password);