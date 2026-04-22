using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;

namespace Musicality.Configurations;

public static class ApiVersioningServiceCollectionExtensions
{
    public static IServiceCollection AddMusicalityApiVersioning(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureApiVersioningOptions>();
        services.ConfigureOptions<ConfigureApiExplorerOptions>();

        services.AddApiVersioning()
            .AddMvc()
            .AddApiExplorer();

        return services;
    }
}
