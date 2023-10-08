using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

public record RenewTokensCommand(string RefreshToken) : IRequest<AuthenticationResult>;