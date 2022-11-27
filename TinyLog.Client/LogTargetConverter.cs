using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyLog.Client;

public class LogTargetConverter : JsonConverter<LogTargets>
{
  public override LogTargets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return reader.GetString().ToLogTargets();
  }

  public override void Write(Utf8JsonWriter writer, LogTargets value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString());
  }
}