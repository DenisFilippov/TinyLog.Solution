using TinyLog.Core;
using TinyLog.Core.Interfaces;

namespace TinyLog.Tests;

public class ItemTests
{
  private Item _item;

  [SetUp]
  public void Setup()
  {
    _item = ItemBuilder
      .Create(10, "100")
      .AddMessage("Child message")
      .AddParent(ItemBuilder
        .Create(10, "100")
        .AddMessage("Parent message")
        .Build())
      .Build();
  }

  [Test]
  public void DataTest()
  {
    Assert.Multiple(() =>
    {
      Assert.That(_item.Id, Is.EqualTo(10));
      Assert.That(_item.Parent, Is.Not.EqualTo(null));
      Assert.That(_item.Parent?.Message, Is.EqualTo("Parent message"));
      Assert.That(_item.ItemType, Is.EqualTo(ItemTypes.Info));
      Assert.That(_item.Message, Is.EqualTo("Child message"));
      Assert.That(_item.RequestId, Is.EqualTo("100"));
    });
  }

  [Test]
  public void XmlSerializeTest()
  {
    IItemSerializer serializer = new ItemXmlSerializer();
    var result = serializer.Serialize(_item);
    var item2 = serializer.Deserialize(result);
    Assert.That(item2, Is.Not.EqualTo(null));
    Assert.That(item2?.Parent, Is.Not.EqualTo(null));
  }

  [Test]
  public void JsonSerializeTest()
  {
    IItemSerializer serializer = new ItemJsonSerializer();
    var result = serializer.Serialize(_item);
    var item2 = serializer.Deserialize(result);
    Assert.That(item2, Is.Not.EqualTo(null));
    Assert.That(item2?.Parent, Is.Not.EqualTo(null));
  }
}