using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<AuthenticationResult>;