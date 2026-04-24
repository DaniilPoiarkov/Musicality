namespace Musicality.ApiVersioning;

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
