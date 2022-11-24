using System.Text.Json;
using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class ItemJsonSerializer : IItemSerializer
{
  public string Serialize(Item item)
  {
    return JsonSerializer.Serialize(item);
  }

  public Item? Deserialize(string value)
  {
    return JsonSerializer.Deserialize<Item>(value);
  }
}