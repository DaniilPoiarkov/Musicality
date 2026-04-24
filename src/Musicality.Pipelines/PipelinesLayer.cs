using Eclipse.Core;
using Eclipse.Core.Pipelines;
using Eclipse.Core.Stores.InMemory;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Musicality.Pipelines;

public static class PipelinesLayer
{
    public static IServiceCollection AddPipelinesLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoreModule(core => core.UseInMemoryStores()
            .ConfigureOptions(options => configuration.GetSection("CoreOptions").Bind(options))
        );

        foreach (var descriptor in services.Where(d => d.ServiceType == typeof(INotFoundPipeline)).ToList())
        {
            services.Remove(descriptor);
        }

        services.AddScoped<INotFoundPipeline, MusicDocumentNotFoundPipeline>();

        return services;
    }
}
