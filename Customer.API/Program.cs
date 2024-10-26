using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Common.Middlewares;
using Customer.API;
using Customer.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.AddBasicMicroserviceFeatures();
// Add services to the container
builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => { opt.SuppressModelStateInvalidFilter = true; });

var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<ICustomerDbContext, CustomerDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();

builder.Services.AddMediatR(typeof(AssemblyMarker).GetTypeInfo().Assembly);

// Other service configurations can go here

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Uncomment if Swagger is needed
    // app.UseSwagger();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clear.General.API v1"));
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Uncomment if using custom middleware
app.UseMiddleware<ExceptionLoggingMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();