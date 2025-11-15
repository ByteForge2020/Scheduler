using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Customer.GrpcService.Services;
using Customer.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddBasicMicroserviceFeatures();
// Add services to the container.
builder.Services.AddGrpc();

var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<ICustomerDbContext, CustomerDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();


var app = builder.Build();

app.MapGrpcService<CustomerGrpcService>();

// Configure the HTTP request pipeline.
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();