using System.Text.Json.Serialization;

namespace Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter<StatusType>))]
public enum StatusType
{
    [JsonStringEnumMemberName("SUCCESS")] Success,
    [JsonStringEnumMemberName("FAILED")] Failed
}