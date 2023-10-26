namespace RhythmicJourney.Application.Features.Identity.Common.DTOs;

/// <summary>
/// Client akauntuna giriwi berpa etmek ucun gondermiw oldugu yeni wifreni temsil edir.
/// </summary>
public record ResetPasswordRequestDTO(string Email, string Password, string PasswordConfirm, string ResetPasswordToken);