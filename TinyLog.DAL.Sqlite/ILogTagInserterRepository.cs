using Microsoft.Data.Sqlite;
using TinyLog.Core;

namespace TinyLog.DAL.Sqlite;

internal interface ILogTagInserterRepository
{
  Task InsertAsync(Item item, SqliteConnection connection, CancellationToken token);
}