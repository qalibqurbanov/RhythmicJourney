using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Register sorgusunu temsil edir.
/// </summary>
/// <param name="FirstName">Userin adi ne olsun?</param>
/// <param name="LastName">Userin soyadi ne olsun?</param>
/// <param name="Email">Userin email adresi ne olsun?</param>
/// <param name="Password">Userin wifresi ne olsun?</param>
public record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<AuthenticationResult>;