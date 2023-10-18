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
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        {
            builder.Services
                .RegisterApplicationServices()
                .RegisterPersistenceServices(builder)
                .RegisterInfrastructureServices(builder.Configuration)
                .RegisterUIServices();
        }

        WebApplication app = builder.Build();
        {
            app.AddMiddlewares();
        }

        app.Run();
    }
}