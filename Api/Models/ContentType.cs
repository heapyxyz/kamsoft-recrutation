using System.Text.Json.Serialization;

namespace Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ContentType>))]
public enum ContentType
{
    [JsonStringEnumMemberName("CSV")] Csv,
    [JsonStringEnumMemberName("INTERNAL_JSON")] Json
}