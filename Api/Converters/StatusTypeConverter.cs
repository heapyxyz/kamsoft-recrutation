using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Models;

namespace Api.Converters;

public class StatusTypeConverter : JsonConverter<StatusType>
{
    public override StatusType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "SUCCESS" => StatusType.Success,
            "FAILED" => StatusType.Failed,
            _ => throw new JsonException($"Unsupported type: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, StatusType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            StatusType.Success => "SUCCESS",
            StatusType.Failed => "FAILED",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        });
    }
}