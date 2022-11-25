using System.Collections.Concurrent;
using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

internal class LogQueue : ILogQueue
{
  private readonly ConcurrentQueue<Item> _queue = new();

  public void Push(Item item)
  {
    ArgumentNullException.ThrowIfNull(item);

    _queue.Enqueue(item);
  }

  public async Task PushAsync(Item item)
  {
    await Task.Factory.StartNew(() => { Push(item); });
  }

  public Item? Pop()
  {
    var result = _queue.TryDequeue(out var item);
    return result ? item : null;
  }

  public async Task<Item?> PopAsync()
  {
    return await Task.Factory.StartNew(Pop);
  }

  public int Count => _queue.Count;
}