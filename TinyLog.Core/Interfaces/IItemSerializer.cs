namespace TinyLog.Core.Interfaces;

public interface IItemSerializer
{
  string Serialize(Item item);

  Item? Deserialize(string value);
}