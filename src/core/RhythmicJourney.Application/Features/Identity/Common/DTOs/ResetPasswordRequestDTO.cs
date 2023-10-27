namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Client akauntuna giriwi berpa etmek ucun gondermiw oldugu yeni wifreni temsil edir.
/// </summary>
/// <param name="Email">Hansi userin wifresini yenileyirik?</param>
/// <param name="Password">Userin yeni wifresi ne olsun?</param>
/// <param name="PasswordConfirm">Userin yeni wifresinin tekrari.</param>
/// <param name="ResetPasswordToken">Wifreni yenileye bilmek ucun serverin usere verdiyi token.</param>
public record ResetPasswordRequestDTO(string Email, string Password, string PasswordConfirm, string ResetPasswordToken);