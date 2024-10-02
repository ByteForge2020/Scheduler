using Common;
using Common.Api.BaseConfiguration;
using Common.Authorization;
using Common.Middlewares;
using General.Application.Queries.GetCustomers;
using General.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.AddBasicMicroserviceFeatures();
// Add services to the container
builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => { opt.SuppressModelStateInvalidFilter = true; });

var connection = builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection);
builder.Services.AddDbContext<GeneralDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IGeneralDbContext, GeneralDbContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, AuthorizationContext>();

builder.Services.AddMediatR(typeof(TestQuery).GetTypeInfo().Assembly);

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

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:17863");
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    options.AllowCredentials();
    options.SetIsOriginAllowed((_) => true);
});

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