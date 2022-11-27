using System.Reflection;
using System.Text.Json.Serialization;

namespace TinyLog.Client;

public class ConfigurationInfo
{
  [JsonConverter(typeof(LogTargetConverter))]
  public LogTargets Target { get; set; }

  public DatabaseInfo Database { get; set; }
}