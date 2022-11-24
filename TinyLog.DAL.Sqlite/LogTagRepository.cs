using System.Data;
using Microsoft.Data.Sqlite;
using TinyLog.Core;

namespace TinyLog.DAL.Sqlite;

internal class LogTagRepository : BaseRepository, ILogTagInserterRepository
{
  private const string LOG_TAGSID = "P_LOG_TAGSID";
  private const string LOG_ITEMSID = "P_LOG_ITEMSID";
  private const string KEY = "P_KEY";
  private const string VALUE = "P_VALUE";
  
  public LogTagRepository(string connectionString) 
    : base(connectionString)
  {
  }
  
  private const string INSERT_SQL =
    $"INSERT INTO LOG_TAGS (LOG_ITEMSID, KEY, VALUE) VALUES (:{LOG_ITEMSID}, :{KEY}, :{VALUE});" +
    "SELECT LAST_INSERT_ROWID()";
  
  private static SqliteCommand CreateInsertCommand(SqliteConnection connection)
  {
    var result = connection.CreateCommand();
    result.CommandText = INSERT_SQL;
    result.CommandType = CommandType.Text;
    var pars = new SqliteParameter[3];
    pars[0] = new SqliteParameter(LOG_ITEMSID, SqliteType.Integer);
    pars[1] = new SqliteParameter(KEY, SqliteType.Integer);
    pars[2] = new SqliteParameter(VALUE, SqliteType.Text) {IsNullable = true};
    result.Parameters.AddRange(pars);
    return result;
  }
  
  private static async Task InsertSingleAsync(long logItemsId, string key, byte[]? value, SqliteCommand command, CancellationToken token)
  {
    command.Parameters[LOG_ITEMSID].Value = logItemsId;
    command.Parameters[KEY].Value = key;
    command.Parameters[VALUE].Value = value != null ? value : DBNull.Value;
    await command.ExecuteNonQueryAsync(token);
  }

  public async Task InsertAsync(Item item, SqliteConnection connection, CancellationToken token)
  {
    await using var command = CreateInsertCommand(connection);
    foreach (var tag in item.Tags)
    {
      await InsertSingleAsync(item.Id, tag.Key, tag.Value, command, token);
    }
  }
}