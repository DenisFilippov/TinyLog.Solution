namespace TinyLog.DAL;

public class OperationException : Exception
{
  public OperationException(string operationName, string? reason = "")
  {
    ArgumentNullException.ThrowIfNull(operationName, nameof(operationName));
    reason ??= string.Empty;

    OperationName = operationName;
    Reason = reason;
  }

  public string OperationName { get; }

  public string Reason { get; }

  public override string ToString()
  {
    return $"OperationName='{OperationName}', Reason='{Reason}'";
  }
}