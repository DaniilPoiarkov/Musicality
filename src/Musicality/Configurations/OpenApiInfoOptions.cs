namespace Musicality.Configurations;

public sealed class OpenApiInfoOptions
{
    public const string SectionName = "OpenApi";

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? ContactUrl { get; set; }
}
