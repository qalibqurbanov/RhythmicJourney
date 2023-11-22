using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Commands;

/// <summary>
/// Rolu silme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Silmek istenilen rol.</param>
public record DeleteRoleCommand(RoleIdentityDTO DTO) : IRequest<RoleResult>;