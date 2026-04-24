using Serilog;

namespace Musicality.Logging;

public static class LoggingExtensions
{
    public static ConfigureHostBuilder AddMusicalityLogging(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, sp, config) =>
            config.ReadFrom.Configuration(
                sp.GetRequiredService<IConfiguration>()
            )
        );

        return host;
    }
}
