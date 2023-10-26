using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Tokenleri yenileme sorgusunu temsil edir.
/// </summary>
/// <param name="RefreshToken">Clientin yeni token ala bilmeyi ucun bize(servere) gonderdiyi Refresh Tokeni temsil edir. </param>
public record RenewTokensCommand(string RefreshToken) : IRequest<AuthenticationResult>;