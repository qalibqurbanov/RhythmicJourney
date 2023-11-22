using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Commands;

/// <summary>
/// Useri roldan silme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Hansi roldan hansi useri cixarmaq isteyirik?</param>
public record DeleteUserFromRoleCommand(UserAndRoleIdentityDTO DTO) : IRequest<RoleResult>;