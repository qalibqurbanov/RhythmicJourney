namespace RhythmicJourney.Core.Constants;

/// <summary>
/// Bu sinif ozunde Identity/Membership ile elaqeli sabit metnleri/mesajlari saxlayir.
/// </summary>
public static partial class IdentityConstants
{
    public static readonly string REGISTER_SUCCESSFUL = "Your registration has been completed successfully. Finally, please confirm your email to activate your account.";
    public static readonly string REGISTER_SUCCESSFUL_AND_CONFIRM_EMAIL = "Congratulations! Your registration was successful. Please confirm your email to complete the registration process.";
    public static readonly string PASSWORD_RESET_SUCCESSFUL = "Congratulations! Your password has been reset, and you're ready to log in.";
    public static readonly string EMAIL_CONFIRM_SUCCESSFUL = "Congratulations, your email is confirmed! Log in and get started.";
    public static readonly string EMAIL_RESET_SENT = "We've sent you an email with instructions for resetting your password. Please check your email.";
    public static readonly string EMAIL_NOT_CONFIRMED = "To start using your account, please confirm your email.";
    public static readonly string EMAIL_CONFIRM_URL_INVALID = "Oops! The confirmation URL is invalid. Please double-check it.";
    public static readonly string LOGOUT_SUCCESSFUL = "You've been logged out successfully.";

    public static readonly string INVALID_CREDENTIALS = "Invalid credentials, please double-check your username and password and try again.";
    public static readonly string DUPLICATE_EMAIL = "Account with this Email already exists.";
    public static readonly string USER_NOT_EXISTS = "The user with this email does not exist in our system, please double-check your email and try logging in again.";

    public static readonly string USER_LOCKED = "Your account is currently locked and inaccessible.";
}

public static partial class IdentityConstants
{
    public static readonly string REFRESH_TOKEN_INVALID = "We couldn't refresh your session due to an invalid Refresh Token.";
    public static readonly string REFRESH_TOKEN_EXPIRED = "Unfortunately, your session cannot be refreshed as the Refresh Token has expired.";

    public static readonly string ACCESS_TOKEN_INVALID = "We're sorry, but your request is currently blocked due to an invalid access token.";
    public static readonly string ACCESS_TOKEN_EXPIRED = "We apologize, but your request can't be processed right now due to an expired access token.";
    public static readonly string ACCESS_TOKEN_HAS_INVALID_ALGHORITM = "We've detected that your access token was signed with a different algorithm.";
    public static readonly string ACCESS_TOKEN_IS_USED = "Your access token is considered invalid because it has been used once. Please obtain a new token for further access.";
}