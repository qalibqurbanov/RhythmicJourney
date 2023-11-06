using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Application.PipelineBehaviors;
using RhythmicJourney.Application.Features.Identity.Validators;
using RhythmicJourney.Application.Features.Identity.Handlers.QueryHandlers;

namespace RhythmicJourney.Application.Extensions;

/// <summary>
/// Application qatinin IoC-ye elave etmeli oldugu servisleri elave eden metodlari saxlayir.
/// </summary>
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

            cfg.RegisterServicesFromAssembly(typeof(LoginQueryHandler).Assembly);
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(LoginQueryValidator).Assembly);

        services.AddHttpContextAccessor();

        return services;
    }
}