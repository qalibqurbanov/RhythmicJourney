using MediatR;
using RhythmicJourney.Application.Features.Identity.Common;

namespace RhythmicJourney.Application.Features.Identity.Commands;

/// <summary>
/// Wifreni berpa etmek ve ya yeni wifre teyin etmek sorgusunu temsil edir.
/// </summary>
/// <param name="Email">Hansi userin wifresini yenileyirik?</param>
/// <param name="Password">Userin yeni wifresi ne olsun?</param>
/// <param name="PasswordConfirm">Userin yeni wifresinin tekrari.</param>
/// <param name="ResetPasswordToken">Wifreni yenileye bilmek ucun serverin usere verdiyi token.</param>
public record ResetPasswordCommand(string Email, string Password, string PasswordConfirm, string ResetPasswordToken) : IRequest<AuthenticationResult>;