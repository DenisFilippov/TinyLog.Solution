using TinyLog.Core.Interfaces;

namespace TinyLog.Core;

public class SearchOptions
{
  public ICouple<DateTime>? DateTimeRange { get; internal set; } = null;

  public string? Application { get; internal set; } = null;

  public string? Writer { get; internal set; } = null;

  public ItemTypes[]? ItemTypes { get; internal set; } = null;

  public bool IsEmpty =>
    DateTimeRange == null
    && string.IsNullOrEmpty(Application)
    && string.IsNullOrEmpty(Writer)
    && (ItemTypes == null || !ItemTypes.Any());
}