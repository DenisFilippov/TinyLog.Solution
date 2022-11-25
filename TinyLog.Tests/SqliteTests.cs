using System.Text;
using TinyLog.Core;
using TinyLog.DAL.Sqlite;

namespace TinyLog.Tests;

internal class SqliteTests
{
  private string _connectionString;
  private Item _item;
  private LogItemRepository _logItemRepository;

  [SetUp]
  public void Setup()
  {
    _connectionString = "Data Source=E:\\Projects\\Db\\DataGripProjects\\TinyLog.Sqlite\\tinyLog.db";

    _logItemRepository = new LogItemRepository(_connectionString);

    _item = ItemBuilder.Create(1L, ItemTypes.Error, "message4")
      .AddApplication("application1")
      .AddParent(
        ItemBuilder.Create(2L, ItemTypes.Error, "message3")
          .AddTag("den", Encoding.UTF8.GetBytes("Hello, Den!!!"))
          .AddTag("maria", Encoding.UTF8.GetBytes("Hello, Maria!!!"))
          .AddData(10L)
          .AddParent(
            ItemBuilder.Create(3L, ItemTypes.Error, "message2")
              .AddData("Hello, world!!!")
              .AddParent(
                ItemBuilder.Create(4L, ItemTypes.Error, "message1")
                  .Build()
              )
              .Build()
          )
          .Build()
      )
      .Build();
  }

  [Test]
  public void ClearTest()
  {
    var source = new CancellationTokenSource();

    _logItemRepository
      .ClearAsync(source.Token)
      .GetAwaiter()
      .GetResult();
  }

  [Test]
  public void InsertTest1()
  {
    var source = new CancellationTokenSource();

    _logItemRepository
      .ClearAsync(source.Token)
      .GetAwaiter()
      .GetResult();
    
    _logItemRepository
      .InsertAsync(_item, source.Token)
      .GetAwaiter()
      .GetResult();
  }

  [Test]
  public void InsertTest2()
  {
    var source = new CancellationTokenSource();
    
    _logItemRepository
      .ClearAsync(source.Token)
      .GetAwaiter()
      .GetResult();

    _logItemRepository
      .InsertAsync(new[] {_item, _item, _item}, source.Token)
      .GetAwaiter()
      .GetResult();
  }
}