using System.Text;
using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class ItemBuilder : IBuilder<Item>
{
  private readonly Item _item;

  private ItemBuilder(long id, ItemTypes itemType, string message, DateTime moment)
  {
    _item = new Item(id, itemType, message, moment);
  }

  private ItemBuilder(long id, ItemTypes itemType, string message)
  {
    _item = new Item(id, itemType, message);
  }

  public Item Build()
  {
    return _item;
  }

  public static ItemBuilder Create(long id, ItemTypes itemType, string message, DateTime moment)
  {
    return new ItemBuilder(id, itemType, message, moment);
  }

  public static ItemBuilder Create(long id, ItemTypes itemType, string message)
  {
    return new ItemBuilder(id, itemType, message);
  }

  public ItemBuilder AddParent(Item parent)
  {
    _item.Parent = parent;
    return this;
  }

  public ItemBuilder AddData(byte[]? data)
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

  public ItemBuilder AddRequestId(string? requestId)
  {
    _item.RequestId = requestId;
    return this;
  }

  public ItemBuilder AddWriter(string? writer)
  {
    _item.Writer = writer;
    return this;
  }

  public ItemBuilder AddApplication(string? application)
  {
    _item.Application = application;
    return this;
  }

  public ItemBuilder AddStacktrace(string? stacktrace)
  {
    _item.StackTrace = stacktrace;
    return this;
  }

  public ItemBuilder AddTag(string key, byte[]? value)
  {
    _item.Tags.Add(key, value);
    return this;
  }

  public ItemBuilder AddTag(string key, string value)
  {
    _item.Tags.Add(key, Encoding.UTF8.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, int value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AAddTag(string key, long value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, bool value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, char value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, double value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, float value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, short value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, uint value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, ulong value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }

  public ItemBuilder AddTag(string key, ushort value)
  {
    _item.Tags.Add(key, BitConverter.GetBytes(value));
    return this;
  }
}