using System.Xml.Serialization;
using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class ItemXmlSerializer : IItemSerializer
{
  private readonly XmlSerializer _serializer = new(typeof(Item));

  public string Serialize(Item item)
  {
    using var writer = new StringWriter();
    _serializer.Serialize(writer, item);
    return writer.ToString();
  }

  public Item? Deserialize(string value)
  {
    using var reader = new StringReader(value);
    return _serializer.Deserialize(reader) as Item;
  }
}