using TinyLog.Core;

namespace TinyLog.DAL;

public interface ILogItemGetterRepository
{
  Task<IEnumerable<Item>> GetAsync(SearchOptions? searchOptions = null);
}