using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Common.Middlewares;
using General.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddBasicMicroserviceFeatures();
var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<GeneralDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGeneralDbContext, GeneralDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<Program>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 15672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.MessageTopology.SetEntityNameFormatter(new CustomEntityNameFormatter("Dev."));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
// Uncomment if using custom middleware
app.UseMiddleware<ExceptionLoggingMiddleware>();

app.Run();