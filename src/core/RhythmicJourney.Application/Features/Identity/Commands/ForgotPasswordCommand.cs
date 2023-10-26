using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Wifreni berpa etmek meqsedile edilmiw sorgunu temsil edir.
/// </summary>
/// <param name="Email">Userin email adresi nedir?</param>
public record ForgotPasswordCommand(string Email) : IRequest<AuthenticationResult>;