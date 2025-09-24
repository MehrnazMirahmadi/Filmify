using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs;
using Filmify.Application.DTOs.Film;
using Filmify.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilmsController(IFilmService filmService) : ControllerBase
{




    // GET: api/Film
    //[HttpGet]
    //public async Task<IActionResult> GetAll([FromQuery] FilmFilterRequest filter)
    //{

    //    var sortOptions = new SortOptions
    //    {
    //        SortBy = "FilmId",
    //        Direction = filter.Direction
    //    };

    //    var result = await filmService.GetFilmsAsync(filter, sortOptions);

    //    if (result.IsRight)
    //        return Ok(result.Right);
    //    else
    //        return NotFound(new { Message = result.Left });

    //}

    // GET: api/Films/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await filmService.GetFilmByIdAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<FilmDto>.Ok(result.Right))
            : NotFound(ApiResponse<FilmDto>.Fail(result.Left));
    }

    // GET: api/Films
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilmFilterRequest filter, [FromQuery] SortOptions? sort = null)
    {
        sort ??= new SortOptions { SortBy = "FilmId", Direction = filter.Direction };
        var result = await filmService.GetFilmsAsync(filter, sort);

        return result.IsRight
            ? Ok(ApiResponse<KeysetPagingResult<FilmDto, long>>.Ok(result.Right))
            : NotFound(ApiResponse<KeysetPagingResult<FilmDto, long>>.Fail(result.Left));
    }

    // POST: api/Films
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FilmCreateDto dto)
    {
        var result = await filmService.CreateFilmAsync(dto);

        return result.IsRight
            ? Ok(ApiResponse<FilmDto>.Ok(result.Right))
            : BadRequest(ApiResponse<FilmDto>.Fail(result.Left));
    }

    // PUT: api/Films/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] FilmUpdateDto dto)
    {
        var result = await filmService.UpdateFilmAsync(id, dto);

        return result.IsRight
            ? Ok(ApiResponse<FilmDto>.Ok(result.Right))
            : NotFound(ApiResponse<FilmDto>.Fail(result.Left));
    }

    // DELETE: api/Films/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await filmService.DeleteFilmAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<bool>.Ok(result.Right))
            : NotFound(ApiResponse<bool>.Fail(result.Left));
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
