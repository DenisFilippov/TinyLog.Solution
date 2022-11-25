using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public static class LogContext
{
  private static ILogQueue _logQueue = new LogQueue();
  
  public static void Push(Item item)
  {
    _logQueue.Push(item);
  }

  public static async Task PushAsync(Item item)
  {
    await Task.Factory.StartNew(() => { Push(item); });
  }

  public static Item? Pop()
  {
    return _logQueue.Pop();
  }

  public static async Task<Item?> PopAsync()
  {
    return await Task.Factory.StartNew(Pop);
  }

  public static int Count => _logQueue.Count;
}