using System;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Text.Json.Serialization;
using RhythmicJourney.WebAPI.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RhythmicJourney.WebAPI.Extensions;

/// <summary>
/// UI qatinin IoC-ye elave etmeli oldugu servisleri elave eden metodlari saxlayir.
/// </summary>
public static class RegisterServices
{
    /// <summary>
    /// IoC Container-a UI qatinin servislerini elave edir.
    /// </summary>
    public static IServiceCollection RegisterUIServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                /* Appimizin dondureceyi json neticenin 'null' olan uzvlerini return etme: */
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                /* Aralarinda 2 terefli elaqe olan entitylerin serializasiyasi zamani "A possible object cycle was detected..." xetasi yaranir. Helli ucun naviqasiyani/referansi propertysini serializasiyadan gizledirik: */
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.AddSwaggerGen(options =>
        {
            {
                options.AddSecurityDefinition("accessToken", new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http,
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
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
            }

            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RhythmicJourney",
                    Description = "API backend supporting CRUD and more.",
                    TermsOfService = new Uri("https://numune.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Elaqe",
                        Url = new Uri("https://numune.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Lisenziya",
                        Url = new Uri("https://numune.com/license")
                    }
                });
            }

            {
                /* Controllerdeki dokumentasiyalar hansi fayla yazilib ve hansi fayldan oxunsun ('Swagger UI'-da gosterilmesi ucun)?: */
                Assembly apiAssembly = typeof(AccountController).Assembly;
                string documentationFileName = $"{apiAssembly.GetName().Name}.xml";
                string documentationFilePath = Path.Combine(AppContext.BaseDirectory, documentationFileName);
                options.IncludeXmlComments(documentationFilePath);
            }
        });

        // services.AddRouting(cfg =>
        services.Configure<RouteOptions>(cfg =>
        {
            cfg.LowercaseUrls = true;
            cfg.LowercaseQueryStrings = false; /* 'true' ola bilmesi ucun 'LowercaseUrls'-de 'true' set olunmalidir. Query String-de token ve s. gonderende ve ya qebul edende problem yaradir deye 'false' olsa yaxwidir. */
        });

        return services;
    }
}