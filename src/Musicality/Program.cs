using Musicality;
using Musicality.Infrastructure;
using Musicality.OpenApi;
using Musicality.Pipelines;
using Musicality.Telegram;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentationLayer()
    .AddInfrastructureLayer()
    .AddPipelinesLayer(builder.Configuration);

var app = builder.Build();

await app.InitializeTelegramBotAsync();

app.UseMusicalitySwagger();

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
