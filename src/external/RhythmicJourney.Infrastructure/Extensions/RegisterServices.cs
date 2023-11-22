using Microsoft.Extensions.Configuration;
using RhythmicJourney.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Infrastructure.Email.Concretes;
using RhythmicJourney.Infrastructure.Identity.Concretes;
using RhythmicJourney.Application.Contracts.Infrastructure.Email.Abstractions;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

namespace RhythmicJourney.Infrastructure.Extensions;

/// <summary>
/// Infrastructure qatinin IoC-ye elave etmeli oldugu servisleri elave eden metodlari saxlayir.
/// </summary>
public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Infrastructure qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        {
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IRefreshTokenValidator, RefreshTokenValidator>();

        }

        {
            HotmailSettings mailSettings = new HotmailSettings();
            configuration.Bind(HotmailSettings.SectionName, mailSettings);
            services.AddSingleton<HotmailSettings>(mailSettings);

            services.AddScoped<IEmailSender, HotmailEmailSender>();
        }

        return services;
    }
}