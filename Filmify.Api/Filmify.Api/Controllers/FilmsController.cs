using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs;
using Filmify.Application.DTOs.Film;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Filmify.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilmsController(IFilmService filmService) : ControllerBase
{

    // GET: api/Films/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        if (id <= 0)
        {
            Log.Warning("Invalid film ID provided: {FilmId}", id);
            return BadRequest(ApiResponse<FilmDto>.Fail("Invalid film ID"));
        }

        try
        {
            Log.Information("Fetching film with ID: {FilmId}", id);
            var result = await filmService.GetFilmByIdAsync(id);

            if (result.IsRight)
            {
                Log.Information("Successfully fetched film with ID: {FilmId}", id);
                return Ok(ApiResponse<FilmDto>.Ok(result.Right));
            }
            else
            {
                Log.Warning("Film not found with ID: {FilmId}", id);
                return NotFound(ApiResponse<FilmDto>.Fail(result.Left));
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching film with ID: {FilmId}", id);
            return StatusCode(500, ApiResponse<FilmDto>.Fail("An error occurred while fetching the film"));
        }
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
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            Log.Warning("Validation failed for film creation: {Errors}", string.Join(", ", errors));
            return BadRequest(ApiResponse<FilmDto>.Fail($"Validation failed: {string.Join(", ", errors)}"));
        }

        try
        {
            Log.Information("Creating new film: {FilmTitle}", dto.FilmTitle);
            var result = await filmService.CreateFilmAsync(dto);

            if (result.IsRight)
            {
                Log.Information("Successfully created film with ID: {FilmId}", result.Right?.FilmId);
                return Ok(ApiResponse<FilmDto>.Ok(result.Right));
            }
            else
            {
                Log.Warning("Failed to create film: {Error}", result.Left);
                return BadRequest(ApiResponse<FilmDto>.Fail(result.Left));
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating film: {FilmTitle}", dto.FilmTitle);
            return StatusCode(500, ApiResponse<FilmDto>.Fail("An error occurred while creating the film"));
        }
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
