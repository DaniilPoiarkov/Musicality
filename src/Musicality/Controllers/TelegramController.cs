using Eclipse.Core.Handlers;

using Microsoft.AspNetCore.Mvc;

using Musicality;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Musicality.Controllers;

[Route("api/telegram")]
[ApiController]
[ServiceFilter(typeof(TelegramSecretTokenFilter))]
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
    public async Task<IActionResult> PostHandle([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            await _activeHandler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update: {UpdateId}", update.Id);
        }

        return Ok();
    }

    [HttpPost("_disabled")]
    public async Task<IActionResult> PostDisabled([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            await _disabledHandler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update (disabled handler): {UpdateId}", update.Id);
        }

        return Ok();
    }
}
