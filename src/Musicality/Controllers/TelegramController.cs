using Asp.Versioning;

using Eclipse.Core.Handlers;

using Microsoft.AspNetCore.Mvc;

using Musicality.Telegram;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Musicality.Controllers;

[ApiController]
[ApiVersion(1.0)]
[TelegramSecretTokenAuthorize]
[Route("api/v{version:apiVersion}/telegram")]
public sealed class TelegramController : ControllerBase
{
    private readonly IEclipseUpdateHandler _activeHandler;

    private readonly IEclipseUpdateHandler _disabledHandler;

    private readonly ILogger<TelegramController> _logger;

    public TelegramController(
        IEnumerable<IEclipseUpdateHandler> updateHandlers,
        ILogger<TelegramController> logger)
    {
        _activeHandler = updateHandlers.Single(h => h.Type == HandlerType.Active);
        _disabledHandler = updateHandlers.Single(h => h.Type == HandlerType.Disabled);
        _logger = logger;
    }

    [HttpPost("_handle")]
    public Task<IActionResult> PostHandle([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        return Handle(_activeHandler, update, botClient, cancellationToken);
    }

    [HttpPost("_disabled")]
    public Task<IActionResult> PostDisabled([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        return Handle(_disabledHandler, update, botClient, cancellationToken);
    }

    [NonAction]
    private async Task<IActionResult> Handle(IEclipseUpdateHandler handler, Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            await handler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update: {UpdateId}", update.Id);
        }

        return NoContent();
    }
}
