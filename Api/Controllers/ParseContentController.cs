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
        return Ok(request.Type);
    }
}