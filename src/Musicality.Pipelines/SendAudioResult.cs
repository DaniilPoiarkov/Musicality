using Eclipse.Core.Results;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Musicality.Pipelines;

internal sealed class SendAudioResult : ResultBase
{
    private readonly string _filePath;
    private readonly string _fileName;

    public SendAudioResult(string filePath, string fileName)
    {
        _filePath = filePath;
        _fileName = fileName;
    }

    public override async Task<Message?> SendAsync(ITelegramBotClient botClient, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var stream = File.OpenRead(_filePath);
            return await botClient.SendAudio(ChatId, InputFile.FromStream(stream, _fileName), cancellationToken: cancellationToken);
        }
        finally
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}
