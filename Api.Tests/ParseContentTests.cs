using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Api.Tests;

public class ParseContentTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("CSV", "123", 0)]
    [InlineData("INTERNAL_JSON", "456", 1)]
    [InlineData("INTERNAL_JSON", "{ \"hello\": \"world\" }", 1)]
    [InlineData("INTERNAL_JSON", "[1, 2, 3, 4]", 4)]
    [InlineData("INTERNAL_JSON", "true", 1)]
    public async Task PostReturnsOk(string type, string content, int expectedCount)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        content = Convert.ToBase64String(bytes);

        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: {type} | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        JsonNode? node = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.Equal("SUCCESS", (string)node!["status"]!);
        Assert.Equal(expectedCount, (int)node["parsed_count"]!);
    }

    [Theory]
    [InlineData(null, "123")]
    [InlineData("CSV", null)]
    [InlineData("something", "else")]
    [InlineData("csv", "...")]
    [InlineData("iNtErNaL_jSoN", "hello_world")]
    [InlineData("INTERNAL_JSON", "this_shouldnt_work")]
    public async Task PostReturnsBadRequest(string? type, string? content)
    {
        if (!string.IsNullOrWhiteSpace(content))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            content = Convert.ToBase64String(bytes);
        }

        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: {type} | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}