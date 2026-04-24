using Musicality.Common.Options;

using System.ComponentModel.DataAnnotations;

namespace Musicality.Telegram;

internal sealed class TelegramOptions : IMusicalityOptions
{
    public static string SectionName => "Telegram";

    [Required]
    public required string Token { get; init; }

    [Required]
    public required string SecretToken { get; init; }

    [Required]
    public required string WebhookUrl { get; init; }
}
