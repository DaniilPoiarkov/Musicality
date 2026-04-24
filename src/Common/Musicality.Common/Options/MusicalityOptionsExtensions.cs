using Microsoft.Extensions.DependencyInjection;

namespace Musicality.Common.Options;

public static class MusicalityOptionsExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMusicalityOptions<T>()
            where T : class, IMusicalityOptions
        {
            services.AddOptions<T>()
                .BindConfiguration(T.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
