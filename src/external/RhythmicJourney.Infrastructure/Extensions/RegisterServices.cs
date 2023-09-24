using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Infrastructure.Authentication;
using RhythmicJourney.Application.Authentication.Abstract;

namespace RhythmicJourney.Infrastructure.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Infrastructure qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}