using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TinyLog.Core;

// TODO : добавить пользовательскую сериализацию/десериализацию в Xml и JSON.
[Serializable]
public class Item
{
  private const string ITEM_TAG = "Item";
  private const string ID_TAG = "Id";
  private const string ITEM_TYPE_TAG = "ItemType";
  private const string MESSAGE_TAG = "Message";
  private const string MOMENT_TAG = "Moment";
  private const string REQUEST_ID_TAG = "RequestId";
  private const string WRITER_TAG = "Writer";
  private const string APPLICATION_TAG = "Application";
  private const string STACK_TRACE_TAG = "StackTrace";
  private const string DATA_TAG = "Data";

  public Item()
  {
    Id = default;
    RequestId = string.Empty;
  }

  public Item(long id, ItemTypes itemType, string message, DateTime moment)
  {
    Id = id;
    ItemType = itemType;
    Message = message;
    Moment = moment;
  }
  
  public Item(long id, ItemTypes itemType, string message)
    :this(id, itemType, message, DateTime.Now)
  {
  }

  public long Id { get; set; }

  public Item? Parent { get; set; }

  public ItemTypes ItemType { get; } = ItemTypes.Info;

  public string Message { get; } = string.Empty;

  public DateTime Moment { get; } = DateTime.Now;

  public string? RequestId { get; set; }

  public string? Writer { get; set; }

  public string? Application { get; set; }

  public string? StackTrace { get; set; }

  public IDictionary<string, byte[]> Tags { get; } = new SerializableDictionary<string, byte[]>();

  public byte[]? Data { get; set; }

  public XmlSchema? GetSchema()
  {
    return null;
  }

  public void ReadXml(XmlReader reader)
  {
  }

  public void WriteXml(XmlWriter writer)
  {
    void WriteSingleItem(Item item, XmlWriter writer)
    {
      writer.WriteStartElement(ID_TAG);
      writer.WriteValue(item.Id);
      writer.WriteEndElement();
      writer.WriteStartElement(ITEM_TYPE_TAG);
      writer.WriteValue(item.ItemType.ToSpecifiedString());
      writer.WriteEndElement();
      writer.WriteStartElement(MESSAGE_TAG);
      writer.WriteValue(item.Message);
      writer.WriteEndElement();
      writer.WriteStartElement(MOMENT_TAG);
      writer.WriteValue(item.Moment);
      writer.WriteEndElement();
      writer.WriteStartElement(REQUEST_ID_TAG);
      writer.WriteValue(item.RequestId);
      writer.WriteEndElement();
      writer.WriteStartElement(WRITER_TAG);
      writer.WriteValue(item.Writer);
      writer.WriteEndElement();
      writer.WriteStartElement(APPLICATION_TAG);
      writer.WriteValue(item.Application);
      writer.WriteEndElement();
      writer.WriteStartElement(STACK_TRACE_TAG);
      writer.WriteValue(item.StackTrace);
      writer.WriteEndElement();
      writer.WriteStartElement(DATA_TAG);
      new XmlSerializer(typeof(byte[])).Serialize(writer, item.Data);
      writer.WriteEndElement();
      (item.Tags as SerializableDictionary<string, byte[]>)?.WriteXml(writer);
    }
    
    Item[] Flatten(Item item)
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

  public override string ToString()
  {
    return Message;
  }
}