using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Common.Api.BaseConfiguration;

public static class LoggingExtension
{
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder, Action<HostBuilderContext, LoggerConfiguration>? configureLogger = null)
    {
        hostBuilder.UseSerilog(
            (ctx, lc) =>
            {
                if (ctx.HostingEnvironment.IsDevelopment())
                {
                    lc.WriteTo.Console();
                    var connectionString = ctx.Configuration.GetConnectionString("DefaultConnection");
                    var sinkOptions = new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlTable = true,
                        TableName = "Logs"
                    };
                        
                    lc.WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: sinkOptions
                    );
                }
                else
                {
                    // perhaps one day...
                }

                lc.ReadFrom.Configuration(ctx.Configuration);
                if (configureLogger is not null)
                {
                    configureLogger(ctx, lc);
                }
            }
        );

        return hostBuilder;
    }
}
