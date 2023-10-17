using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Tokenleri yenileme sorgusunu temsil edir.
/// </summary>
/// <param name="RefreshToken"></param>
public record RenewTokensCommand(string RefreshToken) : IRequest<AuthenticationResult>;