using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Api.Tests;

public class ParseContentTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("CSV", "123")]
    [InlineData("INTERNAL_JSON", "456")]
    public async Task PostReturnsOk(string type, string content)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(content);
        content = Convert.ToBase64String(byteData);

        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: {type} | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(null, "123")]
    [InlineData("CSV", null)]
    [InlineData("something", "else")]
    [InlineData("csv", "...")]
    [InlineData("iNtErNaL_jSoN", "hello_world")]
    public async Task PostReturnsBadRequest(string? type, string? content)
    {
        if (!string.IsNullOrWhiteSpace(content))
        {
            byte[] byteData = Encoding.UTF8.GetBytes(content);
            content = Convert.ToBase64String(byteData);
        }

        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);

        output.WriteLine(
            $"type: {type} | content: {content} | status: {response.StatusCode} | response: {await response.Content.ReadAsStringAsync()}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}