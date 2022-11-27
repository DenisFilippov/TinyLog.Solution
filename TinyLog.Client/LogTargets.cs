namespace TinyLog.Client;

public enum LogTargets
{
  File,
  Database
}

public static class LogTargetsExt
{
  public static LogTargets ToLogTargets(this string? value)
  {
    if (string.IsNullOrEmpty(value))
      throw new ArgumentOutOfRangeException(nameof(value));

    return !Enum.TryParse<LogTargets>(value, out var result) ? throw new FormatException() : result;
  }
}