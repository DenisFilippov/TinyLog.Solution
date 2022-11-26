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

  public async Task PushAsync(Item item, CancellationToken token)
  {
    await Task.Factory.StartNew(() => { Push(item); }, token);
  }

  public Item? Pop()
  {
    var result = _queue.TryDequeue(out var item);
    return result ? item : null;
  }

  public async Task<Item?> PopAsync(CancellationToken token)
  {
    return await Task.Factory.StartNew(() => { return Pop(); }, token);
  }

  public int Count => _queue.Count;
}