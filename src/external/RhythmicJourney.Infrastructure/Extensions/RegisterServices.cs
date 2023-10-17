﻿using Microsoft.Extensions.DependencyInjection;
using RhythmicJourney.Infrastructure.Identity.Concretes;
using RhythmicJourney.Application.Contracts.Infrastructure.Identity.Abstractions;

namespace RhythmicJourney.Infrastructure.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Infrastructure qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IRefreshTokenValidator, RefreshTokenValidator>();
        
        return services;
    }
}