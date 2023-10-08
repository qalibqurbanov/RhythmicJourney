using Microsoft.AspNetCore.Builder;
using RhythmicJourney.WebAPI.Extensions;
using RhythmicJourney.Application.Extensions;
using RhythmicJourney.Persistence.Extensions;
using RhythmicJourney.Infrastructure.Extensions;

namespace RhythmicJourney.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services
                .RegisterApplicationServices()
                .RegisterPersistenceServices(builder.Configuration)
                .RegisterInfrastructureServices()
                .RegisterUIServices();
        }

        var app = builder.Build();
        {
            app.AddMiddlewares();
        }

        app.Run();
    }
}