using MiniExcelLibs;

using Musicality.Common;

namespace Musicality.Infrastructure;

internal sealed class SpreadsheetManager : ISpreadsheetManager
{
    public Task<IEnumerable<T>> Read<T>(Stream stream, string? sheetName = null, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        return stream.QueryAsync<T>(sheetName, cancellationToken: cancellationToken);
    }
}
