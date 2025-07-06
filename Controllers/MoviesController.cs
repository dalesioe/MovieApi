using Microsoft.AspNetCore.Mvc;
using MovieApi.Interfaces;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _service;

    public MoviesController(IMovieService service)
    {
        _service = service;
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> Get(string title)
    {
        var result = await _service.GetMovieInfoAsync(title);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
