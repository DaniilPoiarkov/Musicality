using Musicality.Common.Options;

namespace Musicality.OpenApi;

public static class SwaggerExtensions
{
    public static IServiceCollection AddMusicalitySwagger(this IServiceCollection services)
    {
        services.AddMusicalityOptions<OpenApiInfoOptions>();

        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        services.ConfigureOptions<ConfigureSwaggerUIOptions>();

        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication UseMusicalitySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
