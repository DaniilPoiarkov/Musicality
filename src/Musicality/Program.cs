using Musicality;
using Musicality.Infrastructure;
using Musicality.Pipelines;

using Polly;
using Polly.Contrib.WaitAndRetry;

using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<TelegramSecretTokenFilter>();

builder.Services.AddInfrastructureLayer()
    .AddPipelinesLayer(builder.Configuration);

// TODO: Normalize configuration access.
builder.Services.AddHttpClient<ITelegramBotClient, TelegramBotClient>(
    (client, sp) => new TelegramBotClient(builder.Configuration["Telegram:Token"]!, client)
).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(
    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)
));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var webhookUrl = builder.Configuration["Telegram:WebhookUrl"]
    ?? throw new InvalidOperationException("Cannot start an app without webhook url provided.");

var secretToken = builder.Configuration["Telegram:SecretToken"]
    ?? throw new InvalidOperationException("Secret token for webhook url is not provided.");;

await using (var scope = app.Services.CreateAsyncScope())
{
    var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
    var webhookInfo = await bot.GetWebhookInfo();
    if (webhookInfo.Url != webhookUrl)
    {
        await bot.SetWebhook(
            url: webhookUrl,
            secretToken: secretToken
        );
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
