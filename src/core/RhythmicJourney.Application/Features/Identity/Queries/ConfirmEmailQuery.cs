using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Queries;

/// <summary>
/// Akauntu mail vasitesile tesdiq etme sorgusunu temsil edir.
/// </summary>
public record ConfirmEmailQuery(ConfirmEmailRequestDTO DTO) : IRequest<AuthenticationResult>;