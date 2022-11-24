using System.Text;
using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class ItemBuilder : IBuilder<Item>
{
  private readonly Item _item;

  private ItemBuilder(long id, string requestId)
  {
    _item = new Item(id, requestId);
  }

  public Item Build()
  {
    return _item;
  }

  public static ItemBuilder Create(long id, string requestId)
  {
    return new ItemBuilder(id, requestId);
  }

  public ItemBuilder AddParent(Item parent)
  {
    _item.Parent = parent;
    return this;
  }

  public ItemBuilder AddItemType(ItemTypes itemType)
  {
    _item.ItemType = itemType;
    return this;
  }

  public ItemBuilder AddMessage(string message)
  {
    if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

    _item.Message = message;
    return this;
  }

  public ItemBuilder AddMoment(DateTime moment)
  {
    _item.Moment = moment;
    return this;
  }

  public ItemBuilder AddData(byte[] data)
  {
    _item.Data = data;
    return this;
  }

  public ItemBuilder AddData(string data)
  {
    _item.Data = Encoding.UTF8.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(int data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(long data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(bool data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(char data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(double data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(float data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(short data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(uint data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(ulong data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddData(ushort data)
  {
    _item.Data = BitConverter.GetBytes(data);
    return this;
  }

  public ItemBuilder AddWriter(string writer)
  {
    if (string.IsNullOrEmpty(writer)) throw new ArgumentNullException(nameof(writer));

    _item.Writer = writer;
    return this;
  }

  public ItemBuilder AddApplication(string application)
  {
    if (string.IsNullOrEmpty(application)) throw new ArgumentNullException(nameof(application));

    _item.Application = application;
    return this;
  }

  public ItemBuilder AddStacktrace(string stacktrace)
  {
    if (string.IsNullOrEmpty(stacktrace)) throw new ArgumentNullException(nameof(stacktrace));

    _item.StackTrace = stacktrace;
    return this;
  }

  public ItemBuilder AddTag(string key, byte[] value)
  {
    _item.Tags.Add(key, value);
    return this;
  }
}