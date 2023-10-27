namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Clientin sign-up meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datalari temsil edir.
/// </summary>
/// <param name="FirstName">Userin adi ne olsun?</param>
/// <param name="LastName">Userin soyadi ne olsun?</param>
/// <param name="Email">Userin email adresi ne olsun?</param>
/// <param name="Password">Userin wifresi ne olsun?</param>
public record RegisterRequestDTO(string FirstName, string LastName, string Email, string Password);