using Microsoft.Data.Sqlite;
using TinyORM.Core;
using TinyORM.Sqlite;

namespace TinyLog.DAL.Sqlite;

public class BaseRepository
{
  private readonly string _connectionString;
  protected const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

  static BaseRepository()
  {
    var context = Context.Instance();
    context.SqliteInitialize();
    context.AddEntity(typeof(LogItemEntity));
  }

  public BaseRepository(string connectionString)
  {
    ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
    _connectionString = connectionString;
  }

  protected SqliteConnection CreateConnection()
  {
    return new SqliteConnection(_connectionString);
  }
}