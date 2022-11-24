using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TinyLog.Core;

[Serializable]
public class Item : ISerializable
{
  public Item()
  {
    Id = default;
    RequestId = string.Empty;
  }

  [JsonConstructor]
  public Item(long id, string requestId)
  {
    Id = id;
    RequestId = requestId;
  }

  public Item(SerializationInfo info, StreamingContext context)
  {
    Id = (long) (info.GetValue(nameof(Id), typeof(long)) ??
                 throw new InvalidOperationException($"Property {nameof(Id)} not found."));
    Parent = (Item?) (info.GetValue(nameof(Parent), typeof(Item)) ?? null);
    ItemType = (ItemTypes) (info.GetValue(nameof(ItemType), typeof(ItemTypes)) ??
                            throw new InvalidOperationException($"Property {nameof(ItemType)} not found."));
    Message = (string) (info.GetValue(nameof(Message), typeof(string)) ??
                        throw new InvalidOperationException($"Property {nameof(Message)} not found."));
    Moment = (DateTime) (info.GetValue(nameof(Moment), typeof(DateTime)) ??
                         throw new InvalidOperationException($"Property {nameof(Moment)} not found."));
    RequestId = (string) (info.GetValue(nameof(RequestId), typeof(string)) ??
                          throw new InvalidOperationException($"Property {nameof(RequestId)} not found."));
    Data = (byte[]?) info.GetValue(nameof(Data), typeof(byte[])) ?? null;
    Tags = (IDictionary<string, byte[]>) (info.GetValue(nameof(Tags), typeof(IDictionary<string, byte[]>)) ??
                                          new Dictionary<string, byte[]>());
    Writer = (string) (info.GetValue(nameof(Writer), typeof(string)) ?? null)!;
    Application = (string) (info.GetValue(nameof(Application), typeof(string)) ?? null)!;
    StackTrace = (string) (info.GetValue(nameof(StackTrace), typeof(string)) ?? null)!;
  }

  public long Id { get; set; }

  public Item? Parent { get; set; }

  public ItemTypes ItemType { get; set; } = ItemTypes.Info;

  public string Message { get; set; } = string.Empty;

  public DateTime Moment { get; set; } = DateTime.Now;

  public string RequestId { get; set; }

  public string? Writer { get; set; }

  public string? Application { get; set; }

  public string? StackTrace { get; set; }

  public IDictionary<string, byte[]> Tags { get; } = new Dictionary<string, byte[]>();

  public byte[]? Data { get; set; }

  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue(nameof(Id), Id, typeof(long));
    if (Parent != null) info.AddValue(nameof(Parent), typeof(Item));

    info.AddValue(nameof(ItemType), ItemType, typeof(ItemTypes));
    info.AddValue(nameof(Message), Message, typeof(string));
    info.AddValue(nameof(Moment), Moment, typeof(DateTime));
    info.AddValue(nameof(RequestId), RequestId, typeof(string));
    if (!string.IsNullOrEmpty(Writer)) info.AddValue(nameof(Writer), Writer, typeof(string));

    if (!string.IsNullOrEmpty(Application)) info.AddValue(nameof(Application), Application, typeof(string));

    if (!string.IsNullOrEmpty(StackTrace)) info.AddValue(nameof(StackTrace), StackTrace, typeof(string));

    if (Data != null) info.AddValue(nameof(Data), Data, typeof(object));

    if (Tags.Count > 0) info.AddValue(nameof(Tags), Tags, typeof(IDictionary<string, object>));
  }

  public override string ToString()
  {
    return Message;
  }
}