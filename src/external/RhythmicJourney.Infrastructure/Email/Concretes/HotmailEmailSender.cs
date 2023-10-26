using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using RhythmicJourney.Application.Models;
using RhythmicJourney.Core.Entities.Identity;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;

namespace RhythmicJourney.Infrastructure.Email.Concretes;

/// <summary>
/// Hotmail ile mail gonderme emeliyyatini saxlayir.
/// </summary>
public partial class HotmailEmailSender : IEmailSender
{
    private readonly UserManager<AppUser> _userManager;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HotmailSettings _mailSettings;

    public HotmailEmailSender(
        UserManager<AppUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        LinkGenerator linkGenerator, 
        HotmailSettings mailSettings)
    {
        this._userManager = userManager;
        this._httpContextAccessor = httpContextAccessor;
        this._linkGenerator = linkGenerator;
        this._mailSettings = mailSettings;
    }

    /// <summary>
    /// Bu metod userin akauntunu Mail vasitesile tesdiqlemek ucun mail gonderir.
    /// </summary>
    /// <param name="user">Mail hansi usere gonderilsin?</param>
    public async Task SendConfirmationEmailAsync(AppUser user)
    {
        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        // byte[] bytesOfToken = Encoding.UTF8.GetBytes(token);
        // string encodedBytesOfToken = WebEncoders.Base64UrlEncode(bytesOfToken);

        string confirmationUrl = _linkGenerator.GetUriByAction
        (
              httpContext: _httpContextAccessor.HttpContext,
              action: "ConfirmEmail",
              controller: "Account", /* 'Controller' suffiksi olmamalidir, olsa link generasiya oluna bilinmeyecek. */
              values: new { UserID = user.Id, ConfirmationToken = confirmationToken },
              scheme: _httpContextAccessor.HttpContext.Request.Scheme,
              host: _httpContextAccessor.HttpContext.Request.Host
        );

        await SendEmailAsync
        (
            mailTo: user.Email,
            mailSubject: "Thanks for signing up! Verify your email to complete registration.",
            mailBody: $"Please confirm your account by clicking <a href='{confirmationUrl}'>here</a>."
        );
    }
}

public partial class HotmailEmailSender : IEmailSender
{
    /// <summary>
    /// Bu metod kod tekrarina sebeb olmamaq ucun sirf mail gonderme funksionalligini saxlayan kicik bir komekci metoddur.
    /// </summary>
    /// <param name="mailTo">Hedef email nedir?</param>
    /// <param name="mailSubject">Mailin bawligi ne olsun?</param>
    /// <param name="mailBody">Mailin kontenti ne olsun?</param>
    private async Task SendEmailAsync(string mailTo, string mailSubject, string mailBody)
    {
        using (SmtpClient client = new SmtpClient())
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_mailSettings.DeveloperEmail, _mailSettings.DeveloperPassword);

            client.Host = _mailSettings.Host;
            client.Port = _mailSettings.Port;
            client.EnableSsl = _mailSettings.EnableSSL;
            client.Timeout = _mailSettings.Timeout;

            await client.SendMailAsync
            (
                new MailMessage(_mailSettings.DeveloperEmail, mailTo, mailSubject, mailBody)
                {
                    IsBodyHtml = true
                }
            );
        }
    }
}