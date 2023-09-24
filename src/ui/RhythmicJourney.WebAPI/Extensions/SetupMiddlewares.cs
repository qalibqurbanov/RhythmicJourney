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
        if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}