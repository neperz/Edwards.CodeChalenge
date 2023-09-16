using Edwards.CodeChallenge.Core.Tests.Mocks.Factory;
using Edwards.CodeChallenge.Infra.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
 
using System;
 
using System.Linq;

namespace Edwards.CodeChallenge.Core.Tests.Fixture
{





    public class TestWebApplication
    {
        private static IServiceProvider InitializeServiceProvider(IServiceCollection services)
        {
            services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<EntityContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");

                    }
                );

            services.AddSingleton<DapperContext>(sp =>
            {
                return new DapperContext(MockRepositoryBuilder.GetMockDbConnection().Object);
            });

             

            return services.BuildServiceProvider(); ;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        var descriptor = services.SingleOrDefault(
                                       d => d.ServiceType ==
                                       typeof(DbContextOptions<EntityContext>));

                        services.Remove(descriptor);

                        IServiceProvider sp = InitializeServiceProvider(services);
                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<EntityContext>();


                            db.Database.EnsureCreated();

                            try
                            {
                                new EntityContextSeed(db);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine( "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
                            }
                        }
                    });

                    webBuilder.Configure((hostContext, app) =>
                    {
                        // Configure middleware for testing
                    });
                });
    }

}
