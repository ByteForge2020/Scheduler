using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Common.Middlewares;
using Customer.Contracts;
using General.Application.Commands.CreateCustomer;
using General.Consumer.Consumers;
using General.Infrastructure;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.AddBasicMicroserviceFeatures();
var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<GeneralDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGeneralDbContext, GeneralDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();
builder.Services.AddMediatR(typeof(CreateCustomerCommand).Assembly);
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<CustomerService.CustomerServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["Grpc:CustomerServiceUrl"]);
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<CreateCustomerBusQueryConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 15672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseMiddleware<ExceptionLoggingMiddleware>();

app.Run();