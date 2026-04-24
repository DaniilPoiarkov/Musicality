using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Musicality.OpenApi;

public sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    private readonly IOptions<OpenApiInfoOptions> _openApiInfo;

    public ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider provider,
        IOptions<OpenApiInfoOptions> openApiInfo)
    {
        _provider = provider;
        _openApiInfo = openApiInfo;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfo(description));
        }
    }

    private OpenApiInfo CreateInfo(ApiVersionDescription description)
    {
        var template = _openApiInfo.Value;

        var text = description.IsDeprecated
            ? $"{template.Description} This API version has been deprecated."
            : template.Description;

        var title = string.IsNullOrWhiteSpace(template.Title)
            ? "Musicality API"
            : template.Title;

        var contact = string.IsNullOrWhiteSpace(template.ContactUrl)
            ? null
            : new OpenApiContact { Url = new Uri(template.ContactUrl) };

        return new OpenApiInfo
        {
            Title = title,
            Version = description.ApiVersion.ToString(),
            Description = text,
            Contact = contact
        };
    }
}
