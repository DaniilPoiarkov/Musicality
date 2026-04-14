using Musicality.Common;

using Telegram.Bot;
using Telegram.Bot.Types;

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Musicality.Pipelines;

internal sealed class UpdateHandler : IUpdateHandler
{
    private readonly ISpreadsheetManager _spreadsheetManager;

    public UpdateHandler(ISpreadsheetManager spreadsheetManager)
    {
        _spreadsheetManager = spreadsheetManager;
    }

    public async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default)
    {
        if (update.Message?.Document is null)
        {
            return;
        }

        using var ms = new MemoryStream();

        var fileInfo = await botClient.GetInfoAndDownloadFile(update.Message.Document.FileId, ms, cancellationToken);

        var records = await _spreadsheetManager.Read<MusicUrlRecord>(ms, cancellationToken: cancellationToken);

        using var youtube = new YoutubeClient();

        foreach (var record in records)
        {
            if (string.IsNullOrEmpty(record.Url))
            {
                continue;
            }

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(record.Url, cancellationToken);

            // 3. Select the best audio-only stream (YouTube often separates audio and video)
            var audioStreamInfo = streamManifest.GetAudioOnlyStreams()
                .GetWithHighestBitrate();

            if (audioStreamInfo is null)
            {
                Console.WriteLine("No suitable audio streams found.");
                return;
            }

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var path = Path.Combine(documents, "Musicality");
            var destinationPath = Path.Combine(documents, $"{Path.GetTempFileName()}.mp3");

            // 4. Download and convert the audio stream to an MP3 file
            // The converter handles the FFmpeg integration
            await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, destinationPath);

            Console.WriteLine($"Download complete! Audio saved to: {destinationPath}");

            using var fileStream = File.OpenRead(destinationPath);

            await botClient.SendAudio(update.Message.From!.Id, InputFile.FromStream(fileStream, "test.mp3"));
        }
    }
}

internal sealed class MusicUrlRecord
{
    public string? Url { get; init; }
}
