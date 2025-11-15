using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Common.Middlewares;
using Customer.Contracts;
using General.Application.Queries.GetCustomers;
using General.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.AddBasicMicroserviceFeatures();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(2715);
    options.AddServerHeader = false;
});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => { opt.SuppressModelStateInvalidFilter = true; });
var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<GeneralDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGeneralDbContext, GeneralDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();
builder.Services.AddMediatR(typeof(TestQuery).GetTypeInfo().Assembly);
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<CustomerService.CustomerServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["Grpc:CustomerServiceUrl"]);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clear.General.API v1"));
}

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:17863"); // is it need here? 
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    options.AllowCredentials();
    options.SetIsOriginAllowed((_) => true);
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseMiddleware<ExceptionLoggingMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();