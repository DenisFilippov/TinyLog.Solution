using System.Text.Json;
using TinyLog.Core;
using TinyLog.DAL;

namespace TinyLog.Client;

public static class LogManager
{
  private static string? _connectionString;
  private static string? _directory;
  private static LogTargets _logTarget;
  private static ILogItemInserterRepository? _logItemInserterRepository;

  private static async Task WriteToDatabaseAsync(Item item, CancellationToken token)
  {
    await _logItemInserterRepository?.InsertAsync(item, token)!;
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

  public static Task Initialize(string configFile)
  {
    var content = File.ReadAllText(configFile);
    var configurationInfo = JsonSerializer.Deserialize<ConfigurationInfo>(content);
    if (configurationInfo == null)
      throw new InvalidOperationException();
    
    _logTarget = configurationInfo.Target;
    switch (_logTarget)
    {
      case LogTargets.File:
        break;
      case LogTargets.Database:
        _connectionString = configurationInfo.Database.ConnectionString;
        var inserterClassType = configurationInfo.Database.Assembly.GetTypes()
          .First(r => r.IsAssignableTo(typeof(ILogItemInserterRepository))); 
        _logItemInserterRepository = (ILogItemInserterRepository?) Activator.CreateInstance(inserterClassType, _connectionString);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(_logTarget), _logTarget, null);
    }

    return Task.CompletedTask;
  }
}