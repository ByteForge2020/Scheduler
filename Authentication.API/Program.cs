using Authentication.Application.Commands.CreateUserCommand;
using Authentication.Application.Services.JwtService;
using Authentication.Infrastructure.Data;
using Authentication.Infrastructure.Entities;
using Common;
using Common.Api.BaseConfiguration;
using Common.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(48948); 
    options.AddServerHeader = false;
});

builder.AddBasicMicroserviceFeatures();
// Add services to the container
builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => { opt.SuppressModelStateInvalidFilter = true; });

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(SettingsSectionKey.JwtSettings));

builder.Services.AddDbContext<AuthenticationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(SettingsSectionKey.DatabaseDefaultConnection)));
builder.Services.AddIdentity<SchedulerUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Clear.Authentication.API", Version = "v1"});
});

builder.Services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();