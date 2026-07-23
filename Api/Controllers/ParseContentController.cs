using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/parse-content")]
public class ParseContentController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ParseContent([FromBody] ParseContentModel request)
    {
        return Ok(request.Type);
    }
}