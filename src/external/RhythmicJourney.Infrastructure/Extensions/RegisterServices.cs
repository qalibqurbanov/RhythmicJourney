using Microsoft.Extensions.Configuration;
using RhythmicJourney.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Infrastructure.Email.Concretes;
using RhythmicJourney.Infrastructure.Identity.Concretes;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

namespace RhythmicJourney.Infrastructure.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Infrastructure qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IRefreshTokenValidator, RefreshTokenValidator>();

        MailSettings mailSettings = new MailSettings();
        configuration.Bind(MailSettings.SectionName, mailSettings);
        services.AddScoped<IEmailSender, HotmailEmailSender>(serviceProvider => new HotmailEmailSender
        (
            /* IoC Container-dan 'IEmailSender' teleb olunsa geriye icerisi default olaraq awagidaki datalarla dolu olan 'HotmailEmailSender' orneyi dondururuk: */
            hostAddress:       mailSettings.Host,
            port:              mailSettings.Port,
            enableSSL:         mailSettings.EnableSSL,
            developerEmail:    mailSettings.DeveloperEmail,
            developerPassword: mailSettings.DeveloperPassword
        ));

        return services;
    }
}