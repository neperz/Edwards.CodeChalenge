using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
 
using System.Data.Common;
using Edwards.CodeChallenge.API.Extensions; 

using Edwards.CodeChallenge.API.Filters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NSwag;
using System.IO.Compression;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Edwards.CodeChallenge.API.Services.Interfaces;
using Edwards.CodeChallenge.API.Services;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Domain.Interfaces.Notifications;
using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Interfaces.UoW;
using Edwards.CodeChallenge.Domain.Notifications;
using Edwards.CodeChallenge.Infra.Context;
using Edwards.CodeChallenge.Infra.Repository;
using Edwards.CodeChallenge.Infra.UoW;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Edwards.CodeChallenge.Domain.Models;

var webApplicationOptions = new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory(),
    Args = args,
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.Configuration.AddEnvironmentVariables();

// Configure services
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.AddControllers();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<DomainNotificationFilter>();
    options.EnableEndpointRouting = false;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.Configure<GzipCompressionProviderOptions>(x => x.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(x =>
{
    x.Providers.Add<GzipCompressionProvider>();
});

var hostEnvironment = builder.Environment;

if (hostEnvironment.ApplicationName != "testhost")
{
    var healthCheck = builder.Services.AddHealthChecksUI(setupSettings: setup =>
    {
        setup.DisableDatabaseMigrations();
        setup.MaximumHistoryEntriesPerEndpoint(6);
    }).AddInMemoryStorage();

    var healthCheckBuilder = healthCheck.Services.AddHealthChecks();

    // 500 mb
    healthCheckBuilder.AddProcessAllocatedMemoryHealthCheck(500 * 1024 * 1024, "Process Memory", tags: new[] { "self" });
    // 500 mb
    healthCheckBuilder.AddPrivateMemoryHealthCheck(1500 * 1024 * 1024, "Private memory", tags: new[] { "self" });
}

if (!hostEnvironment.IsProduction())
{
    builder.Services.AddOpenApiDocument(document =>
    {
        document.DocumentName = "v1";
        document.Version = "v1";
        document.Title = "Felipe's CodeChallenge API";
        document.Description = "API CodeChallenge";

        document.PostProcess = (configure) =>
        {
            configure.Info.TermsOfService = "None";
            configure.Info.Contact = new OpenApiContact()
            {
                Name = "Felipe1s Squad",
                Email = "neperz@gmail.com",
                Url = "https://felipe.wikicode.com.br"
            };
            configure.Info.License = new OpenApiLicense()
            {
                Name = "Free to copy",
                Url = "https://felipe.wikicode.com.br"
            };
        };
    });
}

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpContextAccessor();
 
builder.Services.AddScoped<IEdwardsUserService, EdwardsUserService>();
builder.Services.AddSingleton(_ => new ConcurrentDictionary<int, EdwardsUserViewModel>());

builder.Services.AddScoped<IDomainNotification, DomainNotification>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEdwardsUserRepository, EdwardsUserRepository>();

builder.Services.AddDbContext<EntityContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDB")));

builder.Services.AddSingleton<DbConnection>(conn =>
    new SqliteConnection(builder.Configuration.GetConnectionString("UsersDB")));

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<EntityContextSeed>();


// Configure the application
var app = builder.Build();

if (!hostEnvironment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseDatabaseValidation();
    app.UseDatabaseSeed();
}
else
{
    app.UseHsts();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseResponseCompression();

if (!hostEnvironment.IsProduction())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

 

if (hostEnvironment.ApplicationName != "testhost")
{
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = r => r.Tags.Contains("self"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/ready", new HealthCheckOptions
    {
        Predicate = r => r.Tags.Contains("services"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecksUI(setup =>
    {
        setup.UIPath = "/health-ui";
    });
}

app.MapControllers();



app.Run();
