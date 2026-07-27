using System.Text.Json.Nodes;
using Api.Models;

namespace Api.Services;

public class JsonParserService : IContentParser
{
    public ParseResult Parse(string content)
    {
        // JsonNode.Parse(...) returns null if JSON content represents empty object
        JsonNode node = JsonNode.Parse(content) ?? throw new Exception("JSON content is null");

        int count = node switch
        {
            JsonObject obj => obj.Count,
            JsonArray arr => arr.Count,
            _ => 1
        };

        return new ParseResult(count, node);
    }
}