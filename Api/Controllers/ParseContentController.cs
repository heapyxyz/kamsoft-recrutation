using System.Text;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/parse-content")]
public class ParseContentController(JsonParserService jsonParser, CsvParserService csvParser) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    public async Task<IActionResult> ParseContent([FromBody] RequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            ResponseFailedModel failedResponse = new("Field 'content' is null or empty");
            return BadRequest(failedResponse);
        }


        byte[] buffer = new byte[request.Content.Length];
        if (!Convert.TryFromBase64String(request.Content, buffer, out int bytesWritten))
        {
            ResponseFailedModel failedResponse = new("Field 'content' has invalid Base64 data");
            return BadRequest(failedResponse);
        }

        string decodedContent = Encoding.UTF8.GetString(buffer, 0, bytesWritten);

        try
        {
            ParseResult result = request.Type switch
            {
                ContentType.Csv => csvParser.Parse(decodedContent),
                ContentType.Json => jsonParser.Parse(decodedContent),
                _ => throw new Exception("Field 'type' is invalid")
            };

            ResponseSuccessModel successResponse = new(result.ParsedCount, result.ParsedContent);
            return Ok(successResponse);
        }
        catch (Exception e)
        {
            ResponseFailedModel failedResponse = new(e.Message);
            return BadRequest(failedResponse);
        }
    }
}