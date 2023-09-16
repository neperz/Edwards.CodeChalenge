using Edwards.CodeChalenge.API.Filters;
using Edwards.CodeChalenge.API.Services;
using Edwards.CodeChalenge.API.Services.Interfaces;
using Edwards.CodeChalenge.API.ViewModels.User;
using Edwards.CodeChalenge.Domain.Interfaces.Notifications;
using Edwards.CodeChalenge.Domain.Interfaces.Repository;
using Edwards.CodeChalenge.Domain.Interfaces.UoW;
using Edwards.CodeChalenge.Domain.Notifications;
using Edwards.CodeChalenge.Infra.Context;
using Edwards.CodeChalenge.Infra.Repository;
using Edwards.CodeChalenge.Infra.UoW;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using System.Collections.Concurrent;
using System.Data.Common;
using System.IO.Compression;
using System.Text.Json.Serialization;
namespace Edwards.CodeChalenge.API;

public class Startup
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
        _hostEnvironment = hostEnvironment;
    }

     

    public void ConfigureServices(IServiceCollection services)
    {

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        services.AddControllers();
        services.AddMvc(options =>
        {
            options.Filters.Add<DomainNotificationFilter>();
            options.EnableEndpointRouting = false;
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.Configure<GzipCompressionProviderOptions>(x => x.Level = CompressionLevel.Optimal);
        services.AddResponseCompression(x =>
        {
            x.Providers.Add<GzipCompressionProvider>();
        });



        if (_hostEnvironment.ApplicationName != "testhost")
        {
            var healthCheck = services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.DisableDatabaseMigrations();
                setup.MaximumHistoryEntriesPerEndpoint(6);

            }
            ).AddInMemoryStorage();

            var builder = healthCheck.Services.AddHealthChecks();

            //500 mb
            builder.AddProcessAllocatedMemoryHealthCheck(500 * 1024 * 1024, "Process Memory", tags: new[] { "self" });
            //500 mb
            builder.AddPrivateMemoryHealthCheck(1500 * 1024 * 1024, "Private memory", tags: new[] { "self" });

        }

        if (!_webHostEnvironment.IsProduction())
        {
            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1";
                document.Version = "v1";
                document.Title = "Felipe's CodeChalenge API";
                document.Description = "API CodeChalenge";


                document.PostProcess = (configure) =>
                {
                    configure.Info.TermsOfService = "None";
                    configure.Info.Contact = new OpenApiContact()
                    {
                        Name = "Felipe1s Squad",
                        Email = "neperz@gmail.com",
                        Url = "felipe.wikicode.com.br"
                    };
                    configure.Info.License = new OpenApiLicense()
                    {
                        Name = "Free to copy",
                        Url = "exemplo.xyz.com"
                    };
                };


            });
        }

        services.AddAutoMapper(typeof(Startup));
        services.AddHttpContextAccessor();
        this.RegisterServices(services);
        this.RegisterDatabaseServices(services);
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (!env.IsProduction())
        {
            app.UseDeveloperExceptionPage();
            Extensions.WebHostExtensions.ValidateDataBase(app);
            Extensions.WebHostExtensions.TestDatabaseSeed(app);


        }
        else
        {
            app.UseHsts();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseResponseCompression();

        if (!env.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }


        app.UseEndpoints(endpoints =>
        {
            if (_hostEnvironment.ApplicationName != "testhost")
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("self"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("services"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/health-ui";
                });
            }

            endpoints.MapControllers();


        });

    }

    protected virtual void RegisterServices(IServiceCollection services)
    {

        #region Service
        services.AddScoped<IEdwardsUserService, EdwardsUserService>();
        services.AddSingleton(_ => new ConcurrentDictionary<int, EdwardsUserViewModel>());
        #endregion

        #region Domain

        services.AddScoped<IDomainNotification, DomainNotification>();

        #endregion

        #region Infra

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEdwardsUserRepository, EdwardsUserRepository>();


        #endregion
    }

    protected virtual void RegisterDatabaseServices(IServiceCollection services)
    {

        services.AddDbContext<EntityContext>(options =>
            options.UseSqlite(_configuration.GetConnectionString("UsersDB")));
        services.AddSingleton<DbConnection>(conn => new SqliteConnection(_configuration.GetConnectionString("UsersDB")));
        services.AddScoped<DapperContext>();
        services.AddScoped<EntityContextSeed>();




    }


}
