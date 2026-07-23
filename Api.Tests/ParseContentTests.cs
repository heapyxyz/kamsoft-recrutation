using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests;

public class ParseContentTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("CSV", "123")]
    [InlineData("INTERNAL_JSON", "456")]
    public async Task PostReturnsOk(string type, string content)
    {
        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);
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
        var body = new { type, content };
        var response = await _client.PostAsJsonAsync("/api/v1/parse-content", body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}