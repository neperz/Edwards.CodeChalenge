using Edwards.CodeChallenge.API.Extensions;
using Edwards.CodeChallenge.API.Filters;
using Edwards.CodeChallenge.API.Services;
using Edwards.CodeChallenge.API.Services.Interfaces;
using Edwards.CodeChallenge.API.ViewModels.User;
using Edwards.CodeChallenge.Domain.Interfaces;
using Edwards.CodeChallenge.Domain.Interfaces.Notifications;
using Edwards.CodeChallenge.Domain.Interfaces.Repository;
using Edwards.CodeChallenge.Domain.Interfaces.UoW;
using Edwards.CodeChallenge.Domain.Models;
using Edwards.CodeChallenge.Domain.Notifications;
using Edwards.CodeChallenge.Infra;
using Edwards.CodeChallenge.Infra.Context;
using Edwards.CodeChallenge.Infra.Repository;
using Edwards.CodeChallenge.Infra.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.IO;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);

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

//poetic license to academic environment
//if (!hostEnvironment.IsProduction())
//{
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
                Name = "Felipe",
                Email = "neperz@gmail.com",
                Url = "https://github.com/neperz/Edwards.CodeChalenge"
            };
            configure.Info.License = new OpenApiLicense()
            {
                Name = "Free to copy",
                Url = "https://felipe.wikicode.com.br"
            };
        };
    });
//}

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IEdwardsUserService, EdwardsUserService>();
builder.Services.AddSingleton(_ => new ConcurrentDictionary<string, EdwardsUserViewModel>());

builder.Services.Configure<FileConfig>(builder.Configuration.GetSection("FileServiceOptions"));
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IDomainNotification, DomainNotification>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEdwardsUserRepository, EdwardsUserRepository>();

// TODO: bonus - Use SQLite database to store the data instead
builder.Services.AddDbContext<EntityContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("USERDB")));

builder.Services.AddSingleton<DbConnection>(conn =>
    new SqliteConnection(builder.Configuration.GetConnectionString("USERDB")));

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<EntityContextSeed>();



var app = builder.Build();
app.MapGet("/ping", () => new { info = "pong"});

//poetic license to academic environment
//if (!hostEnvironment.IsProduction())
//{
app.UseDeveloperExceptionPage();
app.UseDatabaseValidation();
app.UseDatabaseSeed();
//}
//else
//{
//    app.UseHsts();
//}

app.UseRouting();
//app.UseHttpsRedirection(); //poetic license to facilitate Docker configuration
app.UseResponseCompression();

//poetic license to academic environment
//if (!hostEnvironment.IsProduction())
//{
    app.UseOpenApi();
    app.UseSwaggerUi3();
//}


app.MapControllers();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(context =>
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = context.Request.ContentType;
        context.Response.Body = context.Request.Body;
        return Task.CompletedTask;
    });
});

app.Run();

public partial class Program { }