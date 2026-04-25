using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Musicality.Health;

internal sealed class ApiHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("The API is healthy."));
    }
}
