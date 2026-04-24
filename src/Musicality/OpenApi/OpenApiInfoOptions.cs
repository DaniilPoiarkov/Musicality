using Musicality.Common.Options;

namespace Musicality.OpenApi;

public sealed class OpenApiInfoOptions : IMusicalityOptions
{
    public static string SectionName => "OpenApi";

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? ContactUrl { get; set; }
}
