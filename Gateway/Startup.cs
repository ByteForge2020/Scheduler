using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Security.Cryptography;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins("http://localhost")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((_) => true)));

            services.AddSingleton(provider =>
                {
                    RSA rsa = RSA.Create();
                    var publicKey =
                        @"MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAKKF9y2iN8dunqBdJBHQ8M1LQ4kK3b/mecdbOOs1VSfDa0p7bzFDWUepGV9NyTW0BwE/6gV7Akncc/MQfJdE9n8CAwEAAQ==";
                    rsa.ImportSubjectPublicKeyInfo(
                        Convert.FromBase64String(publicKey),
                        out int _);
                    return new RsaSecurityKey(rsa);
                }
            );

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer(options =>
                    {
                        //it is needed to build service provider here to request security key
                        var securityKey = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
#if DEBUG
                        options.IncludeErrorDetails = true;
#endif
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = securityKey,
                            ValidAudience = "jwt-scheduler",
                            ValidIssuer = "jwt-scheduler",
                            RequireSignedTokens = true,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    }
                );

            services.AddOcelot();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseWebSockets();

            //using customized Ocelot with websocket auth
            await app.UseOcelot();
        }
    }
}