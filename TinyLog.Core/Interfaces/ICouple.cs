namespace TinyLog.Core.Interfaces;

public interface ICouple<T> where T : struct
{
  T? First { get; }

  T? Second { get; }
}