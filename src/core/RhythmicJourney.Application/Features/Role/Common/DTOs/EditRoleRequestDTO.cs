namespace RhythmicJourney.Application.Features.Role.Common.DTOs;

/// <summary>
/// Movcud rol uzerinde deyiwiklik etmek meqsedile bize(servere) POST edilmiw yeni rol datalarini temsil edir.
/// </summary>
/// <param name="newRoleName">Rolun yeni adi ne olsun?</param>
public record EditRoleRequestDTO(string newRoleName);