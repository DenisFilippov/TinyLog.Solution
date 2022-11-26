using TinyLog.Core;
using TinyLog.DAL.Sqlite;

namespace TinyLog.Client;

public static class LogManager
{
  private static string? _connectionString;
  private static string? _directory;
  private static LogTargets _logTarget;
  private static LogItemRepository? _logItemRepository;

  private static async Task WriteToDatabaseAsync(Item item, CancellationToken token)
  {
    await _logItemRepository?.InsertAsync(item, token)!;
  }

  private static async Task WriteToFileAsync(Item item, CancellationToken token)
  {
    throw new NotImplementedException();
  }

  private static Task ProcessAsync(CancellationToken token)
  {
    Task.Factory.StartNew(async () =>
    {
      while (!token.IsCancellationRequested)
      {
        var item = await LogContext.PopAsync(token);
        if (item != null)
          switch (_logTarget)
          {
            case LogTargets.Database:
              await WriteToDatabaseAsync(item, token);
              break;
            case LogTargets.File:
              await WriteToFileAsync(item, token);
              break;
          }

        await Task.Delay(10, token);
      }

      token.ThrowIfCancellationRequested();
    }, token);

    return Task.CompletedTask;
  }

  public static Task Start(CancellationToken token = default)
  {
    return ProcessAsync(token);
  }

  public static Task Initialize(LogTargets logTarget, string additionalData)
  {
    _logTarget = logTarget;
    switch (_logTarget)
    {
      case LogTargets.File:
        _directory = additionalData;
        break;
      case LogTargets.Database:
        _connectionString = additionalData;
        _logItemRepository = new LogItemRepository(_connectionString);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(_logTarget), _logTarget, null);
    }

    return Task.CompletedTask;
  }
}