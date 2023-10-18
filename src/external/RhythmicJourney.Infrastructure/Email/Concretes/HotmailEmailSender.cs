using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;

namespace RhythmicJourney.Infrastructure.Email.Concretes;

/// <summary>
/// Hotmail ile mail gonderme emeliyyatini saxlayir.
/// </summary>
public class HotmailEmailSender : IEmailSender
{
    private string HostAddress       { get; set; }
    private ushort Port              { get; set; }
    private bool   EnableSSL         { get; set; }
    private string DeveloperEmail    { get; set; }
    private string DeveloperPassword { get; set; }

    public HotmailEmailSender(string hostAddress, ushort port, bool enableSSL, string developerEmail, string developerPassword)
    {
        this.HostAddress       = hostAddress;
        this.Port              = port;
        this.EnableSSL         = enableSSL;
        this.DeveloperEmail    = developerEmail;
        this.DeveloperPassword = developerPassword;
    }

    public async Task SendEmailAsync(AppUser user, UserManager<AppUser> userManager, LinkGenerator linkGenerator, HttpContext httpContext)
    {
        string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        // byte[] bytesOfToken = Encoding.UTF8.GetBytes(token);
        // string encodedBytesOfToken = WebEncoders.Base64UrlEncode(bytesOfToken);

        string confirmationUrl = linkGenerator.GetUriByAction
        (
              httpContext: httpContext,
              action:      "ConfirmEmail",
              controller:  "Account", /* 'Controller' suffiksi olmamalidir, olsa link generasiya oluna bilinmeyecek. */
              values:      new { UserID = user.Id, ConfirmationToken = token },
              scheme:      httpContext.Request.Scheme,
              host:        httpContext.Request.Host
        );

        string mailSubject = "Thanks for signing up! Verify your email to complete registration.";
        string mailBody    = $"Please confirm your account by clicking <a href='{confirmationUrl}'>here</a>.";

        using (SmtpClient client = new SmtpClient())
        {
            client.Host                  = this.HostAddress;
            client.Port                  = this.Port;
            client.EnableSsl             = this.EnableSSL;
            client.Timeout               = 60000;
            client.UseDefaultCredentials = false;
            client.Credentials           = new NetworkCredential(this.DeveloperEmail, this.DeveloperPassword);

            await client.SendMailAsync
            (
                new MailMessage(this.DeveloperEmail, user.Email, mailSubject, mailBody)
                {
                    IsBodyHtml = true
                }
            );
        }
    }
}