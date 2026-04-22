using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Musicality.Configurations;

public sealed class ConfigureSwaggerGenOptions(
    IApiVersionDescriptionProvider provider,
    IOptions<OpenApiInfoOptions> openApiInfo)
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;
    private readonly IOptions<OpenApiInfoOptions> _openApiInfo = openApiInfo;

    public void Configure(SwaggerGenOptions options)
    {
        var infoTemplate = _openApiInfo.Value;

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfo(description, infoTemplate));
        }
    }

    private static OpenApiInfo CreateInfo(ApiVersionDescription description, OpenApiInfoOptions template)
    {
        var text = description.IsDeprecated ? $"{template.Description} This API version has been deprecated." : template.Description;

        var info = new OpenApiInfo
        {
            Title = string.IsNullOrWhiteSpace(template.Title) ? "Musicality API" : template.Title,
            Version = description.ApiVersion.ToString(),
            Description = text,
        };

        if (!string.IsNullOrWhiteSpace(template.ContactUrl))
        {
            info.Contact = new OpenApiContact { Url = new Uri(template.ContactUrl) };
        }

        return info;
    }
}
