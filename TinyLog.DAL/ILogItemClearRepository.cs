namespace TinyLog.DAL;

public interface ILogItemClearRepository
{
  Task ClearAsync(CancellationToken token);
}