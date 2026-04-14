using Microsoft.Extensions.DependencyInjection;

using Musicality.Common;

namespace Musicality.Infrastructure;

public static class InfrastructureLayer
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddTransient<ISpreadsheetManager, SpreadsheetManager>();

        return services;
    }
}
