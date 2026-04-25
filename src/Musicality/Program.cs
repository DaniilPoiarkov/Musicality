using Musicality;
using Musicality.Health;
using Musicality.Infrastructure;
using Musicality.Logging;
using Musicality.OpenApi;
using Musicality.Pipelines;
using Musicality.Telegram;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentationLayer()
    .AddInfrastructureLayer()
    .AddPipelinesLayer(builder.Configuration);

builder.Host.AddMusicalityLogging();

var app = builder.Build();

await app.InitializeTelegramBotAsync();

app.UseMusicalitySwagger();

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.UseMusicalityHealthChecks();

await app.RunAsync();
