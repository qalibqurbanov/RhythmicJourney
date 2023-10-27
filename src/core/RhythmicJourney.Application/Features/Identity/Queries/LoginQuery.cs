using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Queries;

/// <summary>
/// Login sorgusunu temsil edir.
/// </summary>
public record LoginQuery(LoginRequestDTO DTO) : IRequest<AuthenticationResult>;