namespace RhythmicJourney.Application.Features.Identity.Common;

/// <summary>
/// Clientin log-in meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datalari temsil edir.
/// </summary>
public record LoginRequestDTO(string Email, string Password);