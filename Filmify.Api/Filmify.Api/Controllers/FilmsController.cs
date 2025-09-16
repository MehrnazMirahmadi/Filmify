using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Film;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilmsController(IFilmService filmService) : ControllerBase
{
  

    // GET: api/Film/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await filmService.GetFilmByIdAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // GET: api/Film
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilmFilterRequest filter)
    {

        var sortOptions = new SortOptions
        {
            SortBy = "FilmId",
            Direction = filter.Direction
        };

        var result = await filmService.GetFilmsAsync(filter, sortOptions);

        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });

    }


    // POST: api/Film
    [HttpPost]
    public async Task<IActionResult> CreateFilm([FromBody] FilmCreateDto dto)
    {
        var result = await filmService.CreateFilmAsync(dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // PUT: api/Film/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFilm(long id, [FromBody] FilmUpdateDto dto)
    {
        var result = await filmService.UpdateFilmAsync(id, dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // DELETE: api/Film/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(long id)
    {
        var result = await filmService.DeleteFilmAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });

    }

    [HttpGet("category/{categoryId:long}")]
    public async Task<IActionResult> GetByCategory(long categoryId, [FromQuery] KeysetPagingRequest paging)
    {
        var result = await filmService.GetFilmsByCategoryAsync(categoryId, paging);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }
  
}
