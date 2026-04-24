using Musicality.ApiVersioning;
using Musicality.OpenApi;
using Musicality.Telegram;

namespace Musicality;

internal static class PresentationLayer
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMusicalityApiVersioning();
        services.AddMusicalitySwagger();
        services.AddTelegramBot();

        return services;
    }
}
