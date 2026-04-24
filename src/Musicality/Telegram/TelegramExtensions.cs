using Microsoft.Extensions.Options;

using Musicality.Common.Options;

using Polly;
using Polly.Contrib.WaitAndRetry;

using Telegram.Bot;

namespace Musicality.Telegram;

public static class TelegramExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
    {
        services.AddMusicalityOptions<TelegramOptions>();
        services.AddScoped<TelegramSecretTokenAuthorize>();

        services.AddHttpClient<ITelegramBotClient, TelegramBotClient>(
            (client, sp) => new TelegramBotClient(sp.GetRequiredService<IOptions<TelegramOptions>>().Value.Token, client)
        ).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(
            Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)
        ));

        return services;
    }

    public static async Task<WebApplication> InitializeTelegramBotAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var options = scope.ServiceProvider.GetRequiredService<IOptions<TelegramOptions>>();
        var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Presentation");

        logger.LogInformation("Initializing webhook for Telegram bot with URL: {WebhookUrl}", options.Value.WebhookUrl);

        var webhookInfo = await bot.GetWebhookInfo(cancellationToken);

        await bot.SetWebhook(
            url: options.Value.WebhookUrl,
            secretToken: options.Value.SecretToken,
            cancellationToken: cancellationToken
        );

        var me = await bot.GetMe(cancellationToken);

        logger.LogInformation("Webhook set successfully. Bot {Bot} running with webhook info: {WebhookInfo}", me.Username, webhookInfo.Url);

        return app;
    }
}
