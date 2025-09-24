using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs;
using Filmify.Application.DTOs.Tag;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(ITagService tagService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await tagService.GetTagByIdAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<TagDto>.Ok(result.Right))
            : NotFound(ApiResponse<TagDto>.Fail(result.Left));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TagFilterRequest filter)
    {
        var result = await tagService.GetTagsAsync(filter);

        return result.IsRight
            ? Ok(ApiResponse<KeysetPagingResult<TagDto, long>>.Ok(result.Right))
            : NotFound(ApiResponse<KeysetPagingResult<TagDto, long>>.Fail(result.Left));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TagCreateDto dto)
    {
        var result = await tagService.CreateTagAsync(dto);

        return result.IsRight
            ? Ok(ApiResponse<TagDto>.Ok(result.Right))
            : BadRequest(ApiResponse<TagDto>.Fail(result.Left));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TagUpdateDto dto)
    {
        var result = await tagService.UpdateTagAsync(id, dto);

        return result.IsRight
            ? Ok(ApiResponse<TagDto>.Ok(result.Right))
            : NotFound(ApiResponse<TagDto>.Fail(result.Left));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await tagService.DeleteTagAsync(id);

        return result.IsRight
            ? Ok(ApiResponse<bool>.Ok(result.Right))
            : NotFound(ApiResponse<bool>.Fail(result.Left));
    }
}
