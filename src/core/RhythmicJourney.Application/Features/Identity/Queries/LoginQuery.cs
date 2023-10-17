using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Queries;

/// <summary>
/// Login sorgusunu temsil edir.
/// </summary>
/// <param name="Email">Userin email adresi nedir?</param>
/// <param name="Password">Userin wifresi nedir?</param>
public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;