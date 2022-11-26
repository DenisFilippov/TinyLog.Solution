using System.Collections;
using TinyLog.Core;

namespace TinyLog.Client;

public class LogClient
{
  public ItemTypes LogLevel { get; set; } = ItemTypes.Info;

  public void Trace(string message)
  {
    Process(ItemTypes.Trace, message);
  }

  public void Info(string message)
  {
    Process(ItemTypes.Info, message);
  }

  public void Hint(string message)
  {
    Process(ItemTypes.Hint, message);
  }

  public void Warning(string message)
  {
    Process(ItemTypes.Warning, message);
  }

  public void Error(string message)
  {
    Process(ItemTypes.Error, message);
  }

  public void Fatal(string message)
  {
    Process(ItemTypes.Fatal, message);
  }

  public void Error(Exception ex)
  {
    ErrorProcess(ItemTypes.Error, ex);
  }

  public void Fatal(Exception ex)
  {
    ErrorProcess(ItemTypes.Fatal, ex);
  }

  public async Task TraceAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Trace, message, token);
  }

  public async Task InfoAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Info, message, token);
  }

  public async Task HintAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Hint, message, token);
  }

  public async Task WarningAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Warning, message, token);
  }

  public async Task ErrorAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Error, message, token);
  }

  public async Task FatalAsync(string message, CancellationToken token)
  {
    await ProcessAsync(ItemTypes.Fatal, message, token);
  }

  public async Task ErrorAsync(Exception ex, CancellationToken token)
  {
    await ErrorProcessAsync(ItemTypes.Error, ex, token);
  }

  public async Task FatalAsync(Exception ex, CancellationToken token)
  {
    await ErrorProcessAsync(ItemTypes.Fatal, ex, token);
  }

  private bool MustLog(ItemTypes itemType)
  {
    return itemType >= LogLevel;
  }

  private static Item CreateItem(ItemTypes itemType, string message)
  {
    return ItemBuilder
      .Create(0L, itemType, message)
      .Build();
  }

  private static Item CreateErrorItem(Exception ex, ItemTypes itemType)
  {
    void AddTag(Exception error, ItemBuilder builder)
    {
      if (error.Data.Count != 0)
        foreach (DictionaryEntry entry in error.Data)
          builder.AddTag((string) entry.Key, (byte[]?) entry.Value);
    }

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
      if (result == null)
      {
        var builder = ItemBuilder
          .Create(0L, itemType, error.Message, moment)
          .AddStacktrace(error.StackTrace)
          .AddApplication(error.Source);
        AddTag(error, builder);
        result = builder.Build();
        current = result;
      }
      else
      {
        var builder = ItemBuilder
          .Create(0L, itemType, error.Message, moment)
          .AddStacktrace(error.StackTrace)
          .AddApplication(error.Source);
        AddTag(error, builder);
        current!.Parent = builder.Build();
        current = current.Parent;
      }

    return result!;
  }

  private void Process(ItemTypes itemType, string message)
  {
    if (!MustLog(itemType)) return;
    LogContext.Push(CreateItem(itemType, message));
  }

  private async Task ProcessAsync(ItemTypes itemType, string message, CancellationToken token)
  {
    if (!MustLog(itemType)) return;
    await LogContext.PushAsync(CreateItem(itemType, message), token);
  }

  private void ErrorProcess(ItemTypes itemType, Exception ex)
  {
    if (!MustLog(itemType)) return;
    LogContext.Push(CreateErrorItem(ex, itemType));
  }

  private async Task ErrorProcessAsync(ItemTypes itemType, Exception ex, CancellationToken token)
  {
    if (!MustLog(itemType)) return;
    await LogContext.PushAsync(CreateErrorItem(ex, itemType), token);
  }
}