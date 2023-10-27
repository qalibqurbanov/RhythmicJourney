using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;
using RhythmicJourney.Application.Features.Identity.Common.DTOs;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Tokenleri yenileme sorgusunu temsil edir.
/// </summary>
public record RenewTokensCommand(RenewTokensRequestDTO DTO) : IRequest<AuthenticationResult>;