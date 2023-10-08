using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using RhythmicJourney.Persistence.Contexts;
using RhythmicJourney.Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RhythmicJourney.Persistence.Repository.Abstract;
using RhythmicJourney.Application.Repository.Abstract;
using RhythmicJourney.Persistence.Repository.Concrete;
using RhythmicJourney.Application.Authentication.Entities;

namespace RhythmicJourney.Persistence.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a Persistence qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSettings jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton<JwtSettings>(jwtSettings);

        services.AddDbContext<RhythmicJourneyIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString_Identity")));

        services.AddIdentity<AppUser, IdentityRole<int>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedEmail = false;

            options.User.RequireUniqueEmail = true;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<RhythmicJourneyIdentityDbContext>();

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