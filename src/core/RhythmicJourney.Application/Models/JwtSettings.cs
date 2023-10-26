namespace RhythmicJourney.Application.Models;

/// <summary>
/// 'appsettings.json'-da saxladigim JWT konfiqurasiyalarini temsil edir.
/// </summary>
public class JwtSettings
{
    public static string SectionName { get; } = nameof(JwtSettings);

    public string AccessTokenSecretKey           { get; init; }
    public string RefreshTokenSecretKey          { get; init; }
    public double AccessTokenExpirationInMinutes { get; init; }
    public double RefreshTokenExpirationInDays   { get; init; }
    public string Issuer                         { get; init; }
    public string Audience                       { get; init; }
}