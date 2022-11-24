using System.Text;
using TinyLog.Core;
using TinyLog.DAL.Sqlite;

namespace TinyLog.Tests;

internal class SqliteTests
{
  private string _connectionString;
  private LogItemRepository _logItemRepository;
  private Item _item;

  [SetUp]
  public void Setup() 
  {
    _connectionString = "Data Source=E:\\Projects\\Db\\DataGripProjects\\TinyLog.Sqlite\\tinyLog.db";

    _logItemRepository = new LogItemRepository(_connectionString);

    _item = ItemBuilder.Create(1L, "11")
      .AddMessage("message1")
      .AddApplication("application1")
      .AddItemType(ItemTypes.Error)
      .AddParent(
        ItemBuilder.Create(2L, "22")
          .AddMessage("message2")
          .AddItemType(ItemTypes.Error)
          .AddTag("den", Encoding.UTF8.GetBytes("Hello, Den!!!"))
          .AddTag("maria", Encoding.UTF8.GetBytes("Hello, Maria!!!"))
          .AddData(10L)
          .AddParent(
            ItemBuilder.Create(3L, "33")
              .AddItemType(ItemTypes.Error)
              .AddMessage("message3")
              .AddData("Hello, world!!!")
              .AddParent(
                ItemBuilder.Create(4L, "44")
                  .AddItemType(ItemTypes.Error)
                  .AddMessage("message4")
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
      .InsertAsync(_item, source.Token)
      .GetAwaiter()
      .GetResult();
  }

  [Test]
  public void InsertTest2()
  {
    var source = new CancellationTokenSource();

    _logItemRepository
      .InsertAsync(new[] {_item, _item, _item}, source.Token)
      .GetAwaiter()
      .GetResult();
  }
}