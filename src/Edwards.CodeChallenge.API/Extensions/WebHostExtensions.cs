using Edwards.CodeChallenge.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Edwards.CodeChallenge.API.Extensions;

[ExcludeFromCodeCoverage]
public static class WebHostExtensions
{
    public static IApplicationBuilder UseDatabaseValidation(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EntityContext>();

        var created = dbContext.Database.EnsureCreated();

        return app;
    }

    public static IApplicationBuilder UseDatabaseSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<EntityContextSeed>();

        seeder.SeedInitial();

        return app;
    }
}
