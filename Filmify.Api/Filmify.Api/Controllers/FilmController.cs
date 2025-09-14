using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Film;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmService;

    public FilmController(IFilmService filmService)
    {
        _filmService = filmService;
    }

    // GET: api/Film/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFilm(long id)
    {
        var result = await _filmService.GetFilmByIdAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // GET: api/Film
    [HttpGet]
    public async Task<IActionResult> GetFilms([FromQuery] FilmFilterRequest filter)
    {

        var sortOptions = new SortOptions
        {
            SortBy = "FilmId",
            Direction = filter.Direction
        };

        var result = await _filmService.GetFilmsAsync(filter, sortOptions);

        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });

    }


    // POST: api/Film
    [HttpPost]
    public async Task<IActionResult> CreateFilm([FromBody] FilmCreateDto dto)
    {
        var result = await _filmService.CreateFilmAsync(dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // PUT: api/Film/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(long id, [FromBody] FilmUpdateDto dto)
    {
        var result = await _filmService.UpdateFilmAsync(id, dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // DELETE: api/Film/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(long id)
    {
        var result = await _filmService.DeleteFilmAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });

    }
}
