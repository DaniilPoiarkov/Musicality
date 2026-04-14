using Telegram.Bot;
using Telegram.Bot.Types;

namespace Musicality.Pipelines;

public interface IUpdateHandler
{
    Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default);
}
