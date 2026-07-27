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

    [Fact]
    public async Task PostJsonObjectReturnsCorrectData()
    {
        string content = "{ \"hello\": \"world\", \"foo\": \"bar\" }";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        content = Convert.ToBase64String(bytes);

        var body = new { type = "INTERNAL_JSON", content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: INTERNAL_JSON | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");

        JsonNode? root = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.Equal(2, (int)root!["parsed_count"]!);
        Assert.Equal("world", (string)root["parsed_content"]!["hello"]!);
        Assert.Equal("bar", (string)root["parsed_content"]!["foo"]!);
    }

    [Fact]
    public async Task PostJsonArrayReturnsCorrectData()
    {
        string content = "[10, 20, 30]";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        content = Convert.ToBase64String(bytes);

        var body = new { type = "INTERNAL_JSON", content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: INTERNAL_JSON | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");

        JsonNode? root = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.Equal(3, (int)root!["parsed_count"]!);
        Assert.Equal(3, ((JsonArray)root["parsed_content"]!).Count);
        Assert.Equal(10, (int)root["parsed_content"]![0]!);
        Assert.Equal(30, (int)root["parsed_content"]![2]!);
    }

    [Fact]
    public async Task PostCsvReturnsCorrectData()
    {
        string content = "name,age\nAlice,30\nBob,25";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        content = Convert.ToBase64String(bytes);

        var body = new { type = "CSV", content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: CSV | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");

        JsonNode? root = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.Equal(2, (int)root!["parsed_count"]!);
        Assert.Equal("Alice", (string)root["parsed_content"]![0]!["name"]!);
        Assert.Equal("25", (string)root["parsed_content"]![1]!["age"]!);
    }
}