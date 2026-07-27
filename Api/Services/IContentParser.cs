using Api.Models;

namespace Api.Services;

public interface IContentParser
{
    ParseResult Parse(string content);
}