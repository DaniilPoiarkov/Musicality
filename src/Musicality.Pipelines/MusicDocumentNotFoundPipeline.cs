using Eclipse.Core.Context;
using Eclipse.Core.Pipelines;
using Eclipse.Core.Results;
using Eclipse.Core.Routing;
using Eclipse.Core.Updates;

using Microsoft.Extensions.Logging;

using Musicality.Common;

using Telegram.Bot;

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Musicality.Pipelines;

/// <summary>
/// Handles document spreadsheet uploads when message text is empty (Core routes those updates to <see cref="INotFoundPipeline"/>).
/// </summary>
[Route("", "")]
internal sealed class MusicDocumentNotFoundPipeline : PipelineBase, INotFoundPipeline
{
    private readonly ISpreadsheetManager _spreadsheetManager;
    private readonly ILogger<MusicDocumentNotFoundPipeline> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateAccessor _updateAccessor;

    public MusicDocumentNotFoundPipeline(
        ISpreadsheetManager spreadsheetManager,
        ILogger<MusicDocumentNotFoundPipeline> logger,
        ITelegramBotClient botClient,
        IUpdateAccessor updateAccessor)
    {
        _spreadsheetManager = spreadsheetManager;
        _logger = logger;
        _botClient = botClient;
        _updateAccessor = updateAccessor;
    }

    protected override void Initialize()
    {
        RegisterStage(ProcessDocumentAsync);
    }

    private async Task<IResult> ProcessDocumentAsync(MessageContext context, CancellationToken cancellationToken)
    {
        var update = _updateAccessor.Update;

        if (update?.Message?.Document is null)
        {
            return Empty();
        }

        var message = update.Message!;
        using var ms = new MemoryStream();

        await _botClient.GetInfoAndDownloadFile(message.Document!.FileId, ms, cancellationToken);

        var records = await _spreadsheetManager.Read<MusicUrlRecord>(ms, cancellationToken: cancellationToken);

        using var youtube = new YoutubeClient();

        var results = new List<IResult>();

        foreach (var record in records)
        {
            if (string.IsNullOrEmpty(record.Url))
            {
                continue;
            }

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(record.Url, cancellationToken);

            var audioStreamInfo = streamManifest.GetAudioOnlyStreams()
                .GetWithHighestBitrate();

            if (audioStreamInfo is null)
            {
                _logger.LogWarning("No suitable audio streams found for URL {Url}", record.Url);
                return Empty();
            }

            var destinationPath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.mp3");

            try
            {
                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, destinationPath, cancellationToken: cancellationToken);
                results.Add(new SendAudioResult(destinationPath, "track.mp3"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download or queue audio for URL {Url}", record.Url);
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                throw;
            }
        }

        return results.Count switch
        {
            0 => Empty(),
            1 => results[0],
            _ => Multiple(results.ToArray()),
        };
    }
}
