using System.Data;
using Microsoft.Data.Sqlite;
using TinyLog.Core;

namespace TinyLog.DAL.Sqlite;

public class LogItemRepository : BaseRepository, ILogItemInserterRepository, ILogItemClearRepository
{
  private const string LOG_ITEMSID = "P_LOG_ITEMSID";
  private const string PARENTID = "P_PARENTID";
  private const string ITEM_TYPEID = "P_ITEM_TYPEID";
  private const string MOMENT = "P_MOMENT";
  private const string MESSAGE = "P_MESSAGE";
  private const string REQUESTID = "P_REQUESTID";
  private const string WRITER = "P_WRITER";
  private const string APPLICATION = "P_APPLICATION";
  private const string STACK_TRACE = "P_STACK_TRACE";
  private const string DATA = "P_DATA";

  private const string INSERT_SQL =
    "INSERT INTO LOG_ITEMS (PARENTID, ITEM_TYPEID, MESSAGE, MOMENT, REQUESTID, WRITER, APPLICATION, STACK_TRACE, DATA) " +
    $"VALUES (:{PARENTID}, :{ITEM_TYPEID}, :{MESSAGE}, :{MOMENT}, :{REQUESTID}, :{WRITER}, :{APPLICATION}, :{STACK_TRACE}, :{DATA});" +
    "SELECT LAST_INSERT_ROWID()";

  private const string CLEAR_SQL = "DELETE FROM LOG_ITEMS";

  private readonly ILogTagInserterRepository _logTagInserterRepository;

  public LogItemRepository(string connectionString)
    : base(connectionString)
  {
    _logTagInserterRepository = new LogTagRepository(connectionString);
  }

  public async Task ClearAsync(CancellationToken token)
  {
    await using var connection = CreateConnection();
    await connection.OpenAsync(token);
    await using var transaction = await connection.BeginTransactionAsync(token);

    try
    {
      await using var command = CreateClearCommand(connection);
      await command.ExecuteNonQueryAsync(token);
      await transaction.CommitAsync(token);
    }
    catch (Exception)
    {
      await transaction.RollbackAsync(token);
      throw;
    }
  }

  public async Task InsertAsync(IEnumerable<Item> items, CancellationToken token)
  {
    await using var connection = CreateConnection();
    await connection.OpenAsync(token);
    await using var transaction = await connection.BeginTransactionAsync(token);

    try
    {
      await using var command = CreateInsertCommand(connection);
      foreach (var item in items)
      foreach (var flattenItem in Flatten(item))
        await InsertSingleAsync(flattenItem, connection, command, token);
      await transaction.CommitAsync(token);
    }
    catch (Exception)
    {
      await transaction.RollbackAsync(token);
      throw;
    }
  }

  public async Task InsertAsync(Item item, CancellationToken token)
  {
    await using var connection = CreateConnection();
    await connection.OpenAsync(token);
    await using var transaction = await connection.BeginTransactionAsync(token);

    try
    {
      await using var command = CreateInsertCommand(connection);
      foreach (var flattenItem in Flatten(item)) await InsertSingleAsync(flattenItem, connection, command, token);
      await transaction.CommitAsync(token);
    }
    catch (Exception)
    {
      await transaction.RollbackAsync(token);
      throw;
    }
  }

  private static SqliteCommand CreateInsertCommand(SqliteConnection connection)
  {
    var result = connection.CreateCommand();
    result.CommandText = INSERT_SQL;
    result.CommandType = CommandType.Text;
    var pars = new SqliteParameter[9];
    pars[0] = new SqliteParameter(PARENTID, SqliteType.Integer) {IsNullable = true};
    pars[1] = new SqliteParameter(ITEM_TYPEID, SqliteType.Integer);
    pars[2] = new SqliteParameter(MESSAGE, SqliteType.Text);
    pars[3] = new SqliteParameter(MOMENT, SqliteType.Text);
    pars[4] = new SqliteParameter(REQUESTID, SqliteType.Text) {IsNullable = true};
    pars[5] = new SqliteParameter(WRITER, SqliteType.Text) {IsNullable = true};
    pars[6] = new SqliteParameter(APPLICATION, SqliteType.Text) {IsNullable = true};
    pars[7] = new SqliteParameter(STACK_TRACE, SqliteType.Text) {IsNullable = true};
    pars[8] = new SqliteParameter(DATA, SqliteType.Blob) {IsNullable = true};
    result.Parameters.AddRange(pars);
    return result;
  }

  private static SqliteCommand CreateClearCommand(SqliteConnection connection)
  {
    var result = connection.CreateCommand();
    result.CommandText = CLEAR_SQL;
    result.CommandType = CommandType.Text;
    return result;
  }

  private async Task InsertSingleAsync(Item item, SqliteConnection connection, SqliteCommand command,
    CancellationToken token)
  {
    command.Parameters[PARENTID].Value = item.Parent != null ? item.Parent.Id : DBNull.Value;
    command.Parameters[ITEM_TYPEID].Value = item.ItemType.ToLong();
    command.Parameters[MESSAGE].Value = item.Message;
    command.Parameters[MOMENT].Value = DateTime.Now.ToString(DATE_TIME_FORMAT);
    command.Parameters[REQUESTID].Value = !string.IsNullOrEmpty(item.RequestId) ? item.RequestId : DBNull.Value;
    command.Parameters[WRITER].Value = !string.IsNullOrEmpty(item.Writer) ? item.Writer : DBNull.Value;
    command.Parameters[APPLICATION].Value = !string.IsNullOrEmpty(item.Application) ? item.Application : DBNull.Value;
    command.Parameters[STACK_TRACE].Value = !string.IsNullOrEmpty(item.StackTrace) ? item.StackTrace : DBNull.Value;
    command.Parameters[DATA].Value = item.Data != null ? item.Data : DBNull.Value;
    item.Id = (long) (await command.ExecuteScalarAsync(token) ?? 0L);

    if (item.Tags.Any()) await _logTagInserterRepository.InsertAsync(item, connection, token);
  }

  private static Item[] Flatten(Item item)
  {
    var result = new List<Item>();
    result.Add(item);
    while (item.Parent != null)
    {
      result.Add(item.Parent);
      item = item.Parent;
    }

    return result.ToArray().Reverse().ToArray();
  }
}