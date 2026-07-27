using System.Text.Json;
using Api.Services;
using Api.Models;

namespace Api.Tests;

public class JsonParserTests
{
    private readonly JsonParserService _service = new();

    [Theory]
    [InlineData(2, "{ \"hello\": \"world\", \"this_is\": \"a_test\" }")]
    public void ParseObjectReturnsCorrectCount(int expectedCount, string content)
    {
        ParseResult result = _service.Parse(content);
        Assert.Equal(expectedCount, result.ParsedCount);
    }

    [Theory]
    [InlineData(5, "[1, 2, 3, 4, 5]")]
    public void ParseArrayReturnsCorrectCount(int expectedCount, string content)
    {
        ParseResult result = _service.Parse(content);
        Assert.Equal(expectedCount, result.ParsedCount);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("10")]
    [InlineData("1")]
    public void ParsePrimitiveReturnsOne(string content)
    {
        ParseResult result = _service.Parse(content);
        Assert.Equal(1, result.ParsedCount);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not json")]
    [InlineData("{ hello: world }")]
    [InlineData("{ \"hello\": \"world }")]
    [InlineData("{ \"hello\": \"world\"")]
    public void ParseInvalidJsonThrowsException(string content)
    {
        Assert.ThrowsAny<JsonException>(() => _service.Parse(content));
    }
}