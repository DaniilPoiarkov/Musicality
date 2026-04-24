using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;

namespace Musicality.ApiVersioning;

public sealed class ConfigureApiExplorerOptions : IConfigureOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
}
