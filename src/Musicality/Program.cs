using Musicality.Infrastructure;
using Musicality.Pipelines;

using Polly;
using Polly.Contrib.WaitAndRetry;

using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddPipelinesLayer()
    .AddInfrastructureLayer();

// TODO: Normalize configuration access.
builder.Services.AddHttpClient<ITelegramBotClient, TelegramBotClient>(
    (client, sp) => new TelegramBotClient(builder.Configuration["Telegram:Token"]!, client)
).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(
    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)
));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

await bot.SetWebhook($"https://set-url/api/telegram/_handle"); // TODO: Set URL.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
