using TinyLog.Core;

namespace TinyLog.DAL;

public interface ILogItemInserterRepository
{
  Task InsertAsync(IEnumerable<Item> items, CancellationToken token);

  Task InsertAsync(Item item, CancellationToken token);
}