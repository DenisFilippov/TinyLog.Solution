namespace TinyLog.Core.Interfaces;

public interface ILogQueue
{
  int Count { get; }
  void Push(Item item);
  Task PushAsync(Item item, CancellationToken token);
  Item? Pop();
  Task<Item?> PopAsync(CancellationToken token);
}