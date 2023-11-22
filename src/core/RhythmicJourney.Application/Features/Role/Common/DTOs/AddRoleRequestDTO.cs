namespace RhythmicJourney.Application.Features.Role.Common.DTOs;

/// <summary>
/// Yeni bir rol yaratmaq meqsedile bize(servere) POST edilmiw datalari temsil edir.
/// </summary>
/// <param name="RoleName">Rolun yeni adi ne olsun?</param>
public record AddRoleRequestDTO(string RoleName);