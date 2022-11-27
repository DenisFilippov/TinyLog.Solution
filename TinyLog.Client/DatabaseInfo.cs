using System.Reflection;
using System.Text.Json.Serialization;

namespace TinyLog.Client;

public class DatabaseInfo
{
  public string ConnectionString { get; set; }
  
  [JsonConverter(typeof(AssemblyConverter))]
  public Assembly Assembly { get; set; }
}