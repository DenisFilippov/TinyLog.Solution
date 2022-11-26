using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TinyLog.Core;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
  public const string TAGS_TAG = "Tags";
  public const string TAG_TAG = "Tag";
  public const string KEY_TAG = "Key";
  public const string VALUE_TAG = "Value";

  public XmlSchema GetSchema()
  {
    return null!;
  }

  public void ReadXml(XmlReader reader)
  {
    if (reader.IsEmptyElement)
      return;

    var keySerializer = new XmlSerializer(typeof(TKey));
    var valueSerializer = new XmlSerializer(typeof(TValue));

    reader.ReadStartElement();
    reader.ReadStartElement();
    while (reader.IsStartElement(TAG_TAG))
    {
      reader.ReadStartElement(TAG_TAG);

      reader.ReadStartElement(KEY_TAG);
      var key = (TKey) keySerializer.Deserialize(reader)!;
      reader.ReadEndElement();

      reader.ReadStartElement(VALUE_TAG);
      var value = (TValue) valueSerializer.Deserialize(reader)!;
      reader.ReadEndElement();

      reader.ReadEndElement();
      Add(key, value);
    }

    reader.ReadEndElement();
    reader.ReadEndElement();
  }

  public void WriteXml(XmlWriter writer)
  {
    var keySerializer = new XmlSerializer(typeof(TKey));
    var valueSerializer = new XmlSerializer(typeof(TValue));

    writer.WriteStartElement(TAGS_TAG);
    foreach (var kvp in this)
    {
      writer.WriteStartElement(TAG_TAG);

      writer.WriteStartElement(KEY_TAG);
      keySerializer.Serialize(writer, kvp.Key);
      writer.WriteEndElement();

      writer.WriteStartElement(VALUE_TAG);
      valueSerializer.Serialize(writer, kvp.Value);
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    writer.WriteEndElement();
  }
}