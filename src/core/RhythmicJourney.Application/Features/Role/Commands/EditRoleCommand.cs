using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Commands;

/// <summary>
/// Rol uzerinde duzeliw etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Rolun yeni melumatlari.</param>
/// <param name="roleIdentityDTO">Hansi rolun melumatlari yenilenecek?</param>
public record EditRoleCommand(EditRoleRequestDTO DTO, RoleIdentityDTO roleIdentityDTO) : IRequest<RoleResult>;