using Microsoft.Extensions.Options;

namespace Musicality.Configurations;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddMusicalitySwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenApiInfoOptions>(configuration.GetSection(OpenApiInfoOptions.SectionName));

        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        services.ConfigureOptions<ConfigureSwaggerUIOptions>();

        services.AddSwaggerGen();

        return services;
    }
}
