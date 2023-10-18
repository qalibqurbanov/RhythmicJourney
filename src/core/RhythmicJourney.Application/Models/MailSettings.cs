namespace RhythmicJourney.Application.Models;

public class MailSettings
{
    public static string SectionName { get; } = "HotmailSettings";

    public string Host              { get; set; }
    public ushort Port              { get; set; }
    public bool   EnableSSL         { get; set; }
    public string DeveloperEmail    { get; set; }
    public string DeveloperPassword { get; set; }
}