using MediatR;
using RhythmicJourney.Application.Features.Role.Common;

namespace RhythmicJourney.Application.Features.Role.Queries;

/// <summary>
/// Rollari elde etme sorgusunu temsil edir.
/// </summary>
public record GetRolesQuery : IRequest<RoleResult>;