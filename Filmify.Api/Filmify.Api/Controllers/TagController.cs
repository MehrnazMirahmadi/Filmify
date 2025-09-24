using Filmify.Application.Common.Paging;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Tag;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(ITagService tagService) : ControllerBase
{
    // GET: api/Tags/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await tagService.GetTagByIdAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // GET: api/Tags
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TagFilterRequest filter)
    {
        var result = await tagService.GetTagsAsync(filter);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // POST: api/Tags
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TagCreateDto dto)
    {
        var result = await tagService.CreateTagAsync(dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return BadRequest(new { Message = result.Left });
    }

    // PUT: api/Tags/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TagUpdateDto dto)
    {
        var result = await tagService.UpdateTagAsync(id, dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    // DELETE: api/Tags/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await tagService.DeleteTagAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }
}
