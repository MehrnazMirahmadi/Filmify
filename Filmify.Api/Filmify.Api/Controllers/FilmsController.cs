using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Film;
using Filmify.Application.Services;
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

    [HttpGet("category/latest/{categoryId:long}")]
    public async Task<IActionResult> GetLatestFilmsByCategory(long categoryId)
    {
        var result = await filmService.GetLatestFilmsByCategoryAsync(categoryId, 6);

        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] FilmSearchRequest request)
    {
        var result = await filmService.SearchFilmsAsync(request);
        return Ok(result);
    }
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 6, [FromQuery] string? searchText = null)
    {
        var result = await filmService.GetPagedFilmsAsync(searchText, pageNumber, pageSize);
        return Ok(result);
    }
}
