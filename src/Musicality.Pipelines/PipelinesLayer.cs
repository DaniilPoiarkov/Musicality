using Microsoft.Extensions.DependencyInjection;

namespace Musicality.Pipelines;

public static class PipelinesLayer
{
    public static IServiceCollection AddPipelinesLayer(this IServiceCollection services)
    {
        // Register pipeline services here

        services.AddTransient<IUpdateHandler, UpdateHandler>();

        return services;
    }
}
