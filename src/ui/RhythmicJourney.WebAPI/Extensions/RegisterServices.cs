using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RhythmicJourney.WebAPI.Extensions;

public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a UI qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterUIServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("accessToken", new OpenApiSecurityScheme
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                BearerFormat = "JWT",
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                /* Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: 'Bearer 12345abcdef'" */
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement /* <=- Bu "Dictionary<OpenApiSecurityScheme, IList<string>>" tipli bir "Dictionary<TKey, TValue>" kolleksiyasidir */
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "accessToken",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }
            });
        });

        // services.AddRouting(cfg =>
        services.Configure<RouteOptions>(cfg =>
        {
            cfg.LowercaseUrls = true; 
            cfg.LowercaseQueryStrings = true; /* 'true' ola bilmesi ucun 'LowercaseUrls'-de 'true' set olunmalidir. */
        });

        return services;
    }
}