using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Musicality.Health;

public static class HealthChecksExtensionss
{
    public static IServiceCollection AddMusicalityHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<ApiHealthCheck>("api", HealthStatus.Unhealthy, ["api"]);

        return services;
    }

    public static WebApplication UseMusicalityHealthChecks(this WebApplication app)
    {
        var options = new HealthCheckOptions
        {
            AllowCachingResponses = true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        };

        app.MapHealthChecks("/_health-checks", options);

        return app;
    }
}
