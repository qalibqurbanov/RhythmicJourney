using Microsoft.Extensions.DependencyInjection;

namespace RhythmicJourney.WebAPI.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a UI qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterUIServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();

        return services;
    }
}