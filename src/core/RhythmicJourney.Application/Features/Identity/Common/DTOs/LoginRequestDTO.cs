namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Clientin log-in meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datalari temsil edir.
/// </summary>
/// <param name="Email">Userin mail adresi nedir?</param>
/// <param name="Password">Userin wifresi nedir?</param>
public record LoginRequestDTO(string Email, string Password);