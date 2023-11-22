using MediatR;
using RhythmicJourney.Application.Features.Role.Common;
using RhythmicJourney.Application.Features.Role.Common.DTOs;

namespace RhythmicJourney.Application.Features.Role.Queries;

/// <summary>
/// X bir roldaki userleri elde etme sorgusunu temsil edir.
/// </summary>
/// <param name="DTO">Hansi roldaki userleri elde edilsin?</param>
public record GetUsersInRoleQuery(RoleIdentityDTO DTO) : IRequest<RoleResult>;