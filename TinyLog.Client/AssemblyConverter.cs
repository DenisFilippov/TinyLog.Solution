using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyLog.Client;

public class AssemblyConverter : JsonConverter<Assembly>
{
  public override Assembly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var assemblyName = reader.GetString();
    return Assembly.Load(assemblyName);
  }

  public override void Write(Utf8JsonWriter writer, Assembly value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.FullName);
  }
}