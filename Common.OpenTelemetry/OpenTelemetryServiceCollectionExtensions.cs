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

        static Uri? BuildEndpointUri(string? endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return null;

            if (Uri.TryCreate(endpoint, UriKind.Absolute, out var absolute))
                return absolute;

            // Support config values like "localhost:4317" by assuming http scheme.
            return Uri.TryCreate($"http://{endpoint}", UriKind.Absolute, out var withScheme)
                ? withScheme
                : null;
        }

        static OtlpExportProtocol ResolveProtocol(Uri endpoint)
            => endpoint.Port == 4317
                ? OtlpExportProtocol.Grpc
                : OtlpExportProtocol.HttpProtobuf;

        var endpointUri = BuildEndpointUri(otlpEndpoint);
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(name))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                if (endpointUri is not null)
                {
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpointUri;
                        options.Protocol = ResolveProtocol(endpointUri);
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

                if (endpointUri is not null)
                {
                    metrics.AddOtlpExporter(options =>
                    {
                        options.Endpoint = endpointUri;
                        options.Protocol = ResolveProtocol(endpointUri);
                    });
                }
            });

        return services;
    }
}
