namespace RhythmicJourney.Application.Authentication.Entities;

public class JwtSettings
{
    public static string SectionName { get; } = "JwtSettings";

    public string SecretKey { get; init; }
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
}