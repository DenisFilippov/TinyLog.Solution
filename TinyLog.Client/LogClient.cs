using TinyLog.Core;

namespace TinyLog.Client;

public class LogClient
{
  public void Trace(string message)
  {
    if (!MustLog(ItemTypes.Trace)) return;
    LogContext.Push(CreateItem(ItemTypes.Trace, message));
  }

  public void Info(string message)
  {
    if (!MustLog(ItemTypes.Info)) return;
    LogContext.Push(CreateItem(ItemTypes.Info, message));
  }

  public void Hint(string message)
  {
    if (!MustLog(ItemTypes.Hint)) return;
    LogContext.Push(CreateItem(ItemTypes.Hint, message));
  }

  public void Warning(string message)
  {
    if (!MustLog(ItemTypes.Warning)) return;
    LogContext.Push(CreateItem(ItemTypes.Warning, message));
  }

  public void Error(string message)
  {
    if (!MustLog(ItemTypes.Error)) return;
    LogContext.Push(CreateItem(ItemTypes.Error, message));
  }

  public void Fatal(string message)
  {
    if (!MustLog(ItemTypes.Fatal)) return;
    LogContext.Push(CreateItem(ItemTypes.Fatal, message));
  }

  public void Error(Exception ex)
  {
    if (!MustLog(ItemTypes.Error)) return;
    LogContext.Push(CreateErrorItem(ex, ItemTypes.Error));
  }

  public void Fatal(Exception ex)
  {
    if (!MustLog(ItemTypes.Fatal)) return;
    LogContext.Push(CreateErrorItem(ex, ItemTypes.Fatal));
  }
  
  public async Task TraceAsync(string message)
  {
    if (!MustLog(ItemTypes.Trace)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Trace, message));
  }

  public async Task InfoAsync(string message)
  {
    if (!MustLog(ItemTypes.Info)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Info, message));
  }

  public async Task HintAsync(string message)
  {
    if (!MustLog(ItemTypes.Hint)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Hint, message));
  }

  public async Task WarningAsync(string message)
  {
    if (!MustLog(ItemTypes.Warning)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Warning, message));
  }

  public async Task ErrorAsync(string message)
  {
    if (!MustLog(ItemTypes.Error)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Error, message));
  }

  public async Task FatalAsync(string message)
  {
    if (!MustLog(ItemTypes.Fatal)) return;
    await LogContext.PushAsync(CreateItem(ItemTypes.Fatal, message));
  }

  public async Task ErrorAsync(Exception ex)
  {
    if (!MustLog(ItemTypes.Error)) return;
    await LogContext.PushAsync(CreateErrorItem(ex, ItemTypes.Error));
  }

  public async Task FatalAsync(Exception ex)
  {
    if (!MustLog(ItemTypes.Fatal)) return;
    await LogContext.PushAsync(CreateErrorItem(ex, ItemTypes.Fatal));
  }

  public ItemTypes LogLevel { get; set; } = ItemTypes.Info;

  private bool MustLog(ItemTypes itemType) => itemType >= LogLevel;

  private static Item CreateItem(ItemTypes itemType, string message)
  {
    return ItemBuilder
      .Create(0L, itemType, message)
      .Build();
  }

  private static Item CreateErrorItem(Exception ex, ItemTypes itemType)
  {
    var errors = new List<Exception>();
    errors.Add(ex);
    while (ex.InnerException != null)
    {
      errors.Add(ex.InnerException);
      ex = ex.InnerException;
    }

    errors.Reverse();
    var errorArray = errors.ToArray();

    var moment = DateTime.Now;
    Item result = null;
    Item current = null;
    foreach (var error in errorArray)
    {
      if (result == null)
      {
        result = ItemBuilder
          .Create(0L, itemType, error.Message, moment)
          .AddStacktrace(error.StackTrace)
          .AddApplication(error.Source)
          .Build();
        current = result;
      }
      else
      {
        current!.Parent = ItemBuilder
          .Create(0L, itemType, error.Message, moment)
          .AddStacktrace(error.StackTrace)
          .AddApplication(error.Source)
          .Build();
        current = current.Parent;
      }
    }

    return result!;
  }
}