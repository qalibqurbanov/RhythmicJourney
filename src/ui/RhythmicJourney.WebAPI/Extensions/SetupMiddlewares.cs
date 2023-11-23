using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RhythmicJourney.WebAPI.Extensions;

public static class SetupMiddlewares
{
    /// <summary>
    /// ASP Core-un execution pipeline-na builtin ve ya custom middleware-leri elave etmekden cavabdehdir.
    /// </summary>
    public static void AddMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/docs.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/docs/v1/docs.json", "RhythmicJourney API v1");
                options.RoutePrefix = "docs";
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}