using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public static class LogContext
{
  private static readonly ILogQueue _logQueue = new LogQueue();

  public static int Count => _logQueue.Count;

  public static void Push(Item item)
  {
    _logQueue.Push(item);
  }

  public static async Task PushAsync(Item item, CancellationToken token)
  {
    await _logQueue.PushAsync(item, token);
  }

  public static Item? Pop()
  {
    return _logQueue.Pop();
  }

  public static async Task<Item?> PopAsync(CancellationToken token)
  {
    return await _logQueue.PopAsync(token);
  }
}