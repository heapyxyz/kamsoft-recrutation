using Api.Services;
using Api.Models;

namespace Api.Tests;

public class CsvParserTests
{
    private readonly CsvParserService _service = new();

    [Theory]
    [InlineData(3, "a,b\n1,2\n3,4\n5,6")]
    [InlineData(1, "a\n1")]
    [InlineData(1, "a,b,c\n1,2")]
    [InlineData(0, "a,b,c")]
    [InlineData(0, "")]
    [InlineData(3, "a\n1\n\n2\n\n\n3")]
    [InlineData(2, "a\n1\n   \n2")]
    [InlineData(1, "a,b\n1")]
    [InlineData(1, "a\n1,2,3")]
    public void ParseReturnsCorrectCount(int expectedCount, string content)
    {
        ParseResult result = _service.Parse(content);
        Assert.Equal(expectedCount, result.ParsedCount);
    }

    [Fact]
    public void ParseBasicCsvReturnsCorrectData()
    {
        ParseResult result = _service.Parse("a,b\n1,2\n3,4");
        var rows = (List<Dictionary<string, string>>)result.ParsedContent;

        Assert.Equal(2, result.ParsedCount);
        Assert.Equal("1", rows[0]["a"]);
        Assert.Equal("2", rows[0]["b"]);
        Assert.Equal("3", rows[1]["a"]);
        Assert.Equal("4", rows[1]["b"]);
    }

    [Fact]
    public void ParseQuotedFieldUnwrapsQuotes()
    {
        ParseResult result = _service.Parse("h\n\"hello, world\"");
        var rows = (List<Dictionary<string, string>>)result.ParsedContent;
        Assert.Equal("hello, world", rows[0]["h"]);
    }

    [Fact]
    public void ParseEscapedQuoteUnescapes()
    {
        ParseResult result = _service.Parse("h\n\"say \"\"hi\"\"\"");
        var rows = (List<Dictionary<string, string>>)result.ParsedContent;
        Assert.Equal("say \"hi\"", rows[0]["h"]);
    }

    [Fact]
    public void ParseFewerFieldsPadsEmpty()
    {
        ParseResult result = _service.Parse("a,b\n1");
        var rows = (List<Dictionary<string, string>>)result.ParsedContent;
        Assert.Equal("1", rows[0]["a"]);
        Assert.Equal("", rows[0]["b"]);
    }
}