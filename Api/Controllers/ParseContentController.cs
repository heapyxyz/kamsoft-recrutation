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
    public async Task<IActionResult> ParseContent([FromBody] ParseContentModel request)
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


        return Ok(decodedContent);
    }
}