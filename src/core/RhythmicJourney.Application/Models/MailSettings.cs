namespace RhythmicJourney.Application.Models;

/// <summary>
/// 'appsettings.json'-da saxladigim Hotmail konfiqurasiyalarini temsil edir.
/// </summary>
public class HotmailSettings
{
    public static string SectionName { get; } = nameof(HotmailSettings);

    public string Host              { get; set; }
    public ushort Port              { get; set; }
    public bool   EnableSSL         { get; set; }
    public ushort Timeout           { get; set; }
    public string DeveloperEmail    { get; set; }
    public string DeveloperPassword { get; set; }
}