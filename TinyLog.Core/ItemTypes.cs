namespace TinyLog.Core;

public enum ItemTypes : long
{
  Trace = 0L,
  Info = 1L,
  Hint = 2L,
  Warning = 3L,
  Error = 4L,
  Fatal = 5L
}

public static class ItemTypesExt
{
  public static string ToSpecifiedString(this ItemTypes value)
  {
    return value switch
    {
      ItemTypes.Error => "ERROR",
      ItemTypes.Fatal => "FATAL",
      ItemTypes.Hint => "HINT",
      ItemTypes.Info => "INFO",
      ItemTypes.Trace => "TRACE",
      ItemTypes.Warning => "WARNING",
      _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };
  }

  public static long ToLong(this ItemTypes value)
  {
    return (long) value;
  }

  public static ItemTypes ToItemType(this long value)
  {
    return (ItemTypes) value;
  }

  public static ItemTypes ToItemType(this string value)
  {
    return value switch
    {
      "ERROR" => ItemTypes.Error,
      "FATAL" => ItemTypes.Fatal,
      "HINT" => ItemTypes.Hint,
      "INFO" => ItemTypes.Info,
      "TRACE" => ItemTypes.Trace,
      "WARNING" => ItemTypes.Warning,
      _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };
  }
}