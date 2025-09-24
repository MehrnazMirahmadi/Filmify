using Filmify.Application.Common.Paging;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs;
using Filmify.Application.DTOs.Box;
using global::Filmify.Application.Common.Sorting;
using Microsoft.AspNetCore.Mvc;
namespace Filmify.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BoxesController(IBoxService boxService) : ControllerBase
{
    // GET: api/Boxes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await boxService.GetBoxByIdAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<BoxDto>.Ok(result.Right))
            : NotFound(ApiResponse<BoxDto>.Fail(result.Left));
    }

    // GET: api/Boxes
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BoxFilterRequest filter, [FromQuery] SortOptions? sort = null)
    {
        var result = await boxService.GetBoxesAsync(filter, sort);

        return result.IsRight
            ? Ok(ApiResponse<KeysetPagingResult<BoxDto, long>>.Ok(result.Right))
            : NotFound(ApiResponse<KeysetPagingResult<BoxDto, long>>.Fail(result.Left));
    }

    // POST: api/Boxes
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BoxCreateDto dto)
    {
        var result = await boxService.CreateBoxAsync(dto);

        return result.IsRight
            ? Ok(ApiResponse<BoxDto>.Ok(result.Right))
            : BadRequest(ApiResponse<BoxDto>.Fail(result.Left));
    }

    // PUT: api/Boxes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] BoxUpdateDto dto)
    {
        var result = await boxService.UpdateBoxAsync(id, dto);

        return result.IsRight
            ? Ok(ApiResponse<BoxDto>.Ok(result.Right))
            : NotFound(ApiResponse<BoxDto>.Fail(result.Left));
    }

    // DELETE: api/Boxes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await boxService.DeleteBoxAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<bool>.Ok(result.Right))
            : NotFound(ApiResponse<bool>.Fail(result.Left));
    }
}

