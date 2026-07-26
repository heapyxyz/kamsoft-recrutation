using System.Text;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/parse-content")]
public class ParseContentController : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> ParseContent([FromBody] RequestModel request)
    {
        string decodedContent;
        try
        {
            byte[] byteData = Convert.FromBase64String(request.Content);
            decodedContent = Encoding.UTF8.GetString(byteData);
        }
        catch
        {
            return BadRequest("Field 'content' has invalid Base64 data");
        }

        ResponseModel response = new(StatusType.Success, 123, new { Hello = "World" });
        return Ok(response);
    }
}