using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Common.OpenTelemetry;

public static class OpenTelemetryServiceCollectionExtensions
{
    /// <summary>
    /// Registers tracing and metrics (ASP.NET Core, HttpClient, runtime) with OTLP export when
    /// <c>OpenTelemetry:OtlpEndpoint</c> is set, otherwise console trace export in Development only.
    /// Set <c>OpenTelemetry:Enabled</c> to false to disable.
    /// </summary>
    public static IServiceCollection AddSchedulerOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var section = configuration.GetSection("OpenTelemetry");
        if (!section.GetValue("Enabled", true))
            return services;

        var name = section["ServiceName"] ?? serviceName;
        var otlpEndpoint = section["OtlpEndpoint"];
        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? Environments.Production;
        var useConsoleTraces = string.IsNullOrWhiteSpace(otlpEndpoint)
            && string.Equals(environment, Environments.Development, StringComparison.OrdinalIgnoreCase);
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(name))
            .WithTracing(tracing =>
            {
                tracing
                    .SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otlpEndpoint);
                    });
                }
                else if (useConsoleTraces)
                {
                    tracing.AddConsoleExporter();
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                {
                    metrics.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otlpEndpoint);
                    });
                }
            });

        return services;
    }
}
