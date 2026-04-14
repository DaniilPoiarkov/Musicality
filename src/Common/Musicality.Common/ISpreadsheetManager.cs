namespace Musicality.Common;

public interface ISpreadsheetManager
{
    public Task<IEnumerable<T>> Read<T>(Stream stream, string? sheetName = null, CancellationToken cancellationToken = default)
        where T : class, new();
}
