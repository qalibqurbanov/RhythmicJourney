using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RhythmicJourney.Application.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Application qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.Lifetime = ServiceLifetime.Scoped; /* Elave edilen servislerin omru ne qeder olsun? default: 'Transient' (Requst ve Notification handlerleri 'Scoped', 'Behavior'-lar ise 'Transient' olmalidir) */

            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}