using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class Couple<T> : ICouple<T> where T : struct
{
  public Couple(T? first, T? second)
  {
    First = first;
    Second = second;
  }

  public T? First { get; }
  public T? Second { get; }
}