namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Clientin yeni 'Access Token' ve 'Refresh Token' elde etmek meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu 'Refresh Token'-i temsil edir.
/// </summary>
/// <param name="RefreshToken">Clientin yeni token ala bilmeyi ucun bize(servere) gonderdiyi Refresh Tokeni temsil edir. </param>
public record RenewTokensRequestDTO(string RefreshToken);