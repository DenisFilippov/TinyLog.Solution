using System.Text;
using TinyLog.Core;
using TinyLog.Core.Interfaces;

namespace TinyLog.Tests;

public class ItemTests
{
  private Item _item1;
  private Item _item2;

  [SetUp]
  public void Setup()
  {
    _item1 = ItemBuilder
      .Create(10, ItemTypes.Error, "Child message")
      .AddParent(ItemBuilder
        .Create(10, ItemTypes.Error, "Parent message")
        .Build())
      .Build();

    _item2 = ItemBuilder.Create(1L, ItemTypes.Info, "message1")
      .AddApplication("application1")
      .AddParent(
        ItemBuilder.Create(2L, ItemTypes.Error, "message2")
          .AddTag("den", Encoding.UTF8.GetBytes("Hello, Den!!!"))
          .AddTag("maria", Encoding.UTF8.GetBytes("Hello, Maria!!!"))
          .AddData(10L)
          .AddParent(
            ItemBuilder.Create(3L, ItemTypes.Error, "message3")
              .AddData("Hello, world!!!")
              .AddParent(
                ItemBuilder.Create(4L, ItemTypes.Error, "message4")
                  .Build()
              )
              .Build()
          )
          .Build()
      )
      .Build();
  }

  [Test]
  public void DataTest()
  {
    Assert.Multiple(() =>
    {
      Assert.That(_item1.Id, Is.EqualTo(10));
      Assert.That(_item1.Parent, Is.Not.EqualTo(null));
      Assert.That(_item1.Parent?.Message, Is.EqualTo("Parent message"));
      Assert.That(_item1.ItemType, Is.EqualTo(ItemTypes.Info));
      Assert.That(_item1.Message, Is.EqualTo("Child message"));
      Assert.That(_item1.RequestId, Is.EqualTo("100"));
    });
  }

  [Test]
  public void XmlSerializeTest()
  {
    IItemSerializer serializer = new ItemXmlSerializer();
    var result = serializer.Serialize(_item2);
    var item2 = serializer.Deserialize(result);
    Assert.That(item2, Is.Not.EqualTo(null));
    Assert.That(item2?.Parent, Is.Not.EqualTo(null));
  }

  [Test]
  public void JsonSerializeTest()
  {
    IItemSerializer serializer = new ItemJsonSerializer();
    var result = serializer.Serialize(_item2);
    var item2 = serializer.Deserialize(result);
    Assert.That(item2, Is.Not.EqualTo(null));
    Assert.That(item2?.Parent, Is.Not.EqualTo(null));
  }
}