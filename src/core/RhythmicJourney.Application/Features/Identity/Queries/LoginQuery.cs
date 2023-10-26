using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Queries;

/// <summary>
/// Login sorgusunu temsil edir.
/// </summary>
public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;