using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Commands;

/// <summary>
/// Useri rola elave etme sorgusunu temsil edir.
/// </summary>
public record AddUserToRoleCommand(UserAndRoleIdentityDTO DTO) : IRequest<RoleResult>;