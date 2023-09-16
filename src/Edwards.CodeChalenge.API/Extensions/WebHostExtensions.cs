using Edwards.CodeChalenge.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Edwards.CodeChalenge.API.Extensions;

[ExcludeFromCodeCoverage]
public static class WebHostExtensions
{
    public static IWebHost SeedData(this IWebHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<EntityContext>();

            context.Database.Migrate();

            new EntityContextSeed(context);
        }

        return host;
    }
    public static void ValidateDataBase(IApplicationBuilder app)
    {

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EntityContext>();

        var created = dbContext.Database.EnsureCreated();



    }
    public static void TestDatabaseSeed(IApplicationBuilder app)
    {

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var seeder = scope.ServiceProvider.GetRequiredService<EntityContextSeed>();

        seeder.SeedInitial();




    }
}
