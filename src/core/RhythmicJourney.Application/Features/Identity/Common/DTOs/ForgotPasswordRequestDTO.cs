namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Client akauntunun wifresini yaddan cixarmasi kimi sebebe gore akaunta giriwini berpa etmek meqsedile bize(servere) HTTP POST Request vasitesile gondermiw oldugu datani temsil edir.
/// </summary>
public record ForgotPasswordRequestDTO(string Email);