using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RhythmicJourney.Application.Models;
using Microsoft.Extensions.Configuration;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Persistence.Repositories.Concretes;
using RhythmicJourney.Application.Contracts.Persistence.Repositories.Abstractions;

namespace RhythmicJourney.Persistence.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Persistence qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        JwtSettings jwtSettings = new JwtSettings();
        builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton<JwtSettings>(jwtSettings);

        services.AddDbContext<RhythmicJourneyIdentityDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString_Identity"), options =>
            {
                options.CommandTimeout(30);
            });

            if (builder.Environment.IsDevelopment()) /* Production-da iwletmirik, cunki DB-miz ve s. ile elaqeli gizli qalmali olan melumatlarida gostere biler. */
            {
                options.EnableDetailedErrors(true);
                options.EnableSensitiveDataLogging(true);
            }
            else
            {
                options.EnableDetailedErrors(false);
                options.EnableSensitiveDataLogging(false);
            }
        });

        services.AddIdentity<AppUser, IdentityRole<int>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
        })
            .AddEntityFrameworkStores<RhythmicJourneyIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(1));
        /*
            * Def: 24 hours.
            > "https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/DataProtectionTokenProviderOptions.cs".
        */

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecretKey)),
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    /* Userin autentifikasiya isteyinin ugursuz sonlanmagina sebeb 'Access Token'-inin omrunun qurtarmagidirsa bu zaman Response-a yeni custom bir Header elave edirik. Client serverden elde etdiyi hemin bu custom Response Header vasitesile 'Access Token'-in omrunun qurtardigini bawa duwecek ve yeni 'Access Token' ve 'Refresh Token' elde etmek ucun uygun endpointe('/renew-tokens') muraciet edecek: */
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}