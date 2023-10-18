using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Queries;

public record ConfirmEmailQuery(string UserID, string ConfirmationToken) : IRequest<AuthenticationResult>;