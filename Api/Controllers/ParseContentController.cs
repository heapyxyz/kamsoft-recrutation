using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/parse-content")]
public class ParseContentController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ParseContenet()
    {
        return Ok("Hello, world!");
    }
}