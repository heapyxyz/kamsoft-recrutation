using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Models;

namespace Api.Converters;

public class ContentTypeConverter : JsonConverter<ContentType>
{
    public override ContentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "CSV" => ContentType.Csv,
            "INTERNAL_JSON" => ContentType.Json,
            _ => throw new JsonException($"Unsupported type: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ContentType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            ContentType.Csv => "CSV",
            ContentType.Json => "INTERNAL_JSON",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        });
    }
}