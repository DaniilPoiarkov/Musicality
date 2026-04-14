using Microsoft.AspNetCore.Mvc;

using Musicality.Pipelines;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Musicality.Controllers;

[Route("api/telegram")]
[ApiController]
public sealed class TelegramController : ControllerBase
{
    private readonly IUpdateHandler _updateHandler;

    private readonly ILogger<TelegramController> _logger;

    public TelegramController(IUpdateHandler updateHandler, ILogger<TelegramController> logger)
    {
        _updateHandler = updateHandler;
        _logger = logger;
    }

    [HttpPost("_handle")]
    public async Task<IActionResult> Post([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            await _updateHandler.Handle(botClient, update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update: {UpdateId}", update.Id);
        }

        return Ok();
    }
}
