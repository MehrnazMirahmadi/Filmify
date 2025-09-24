using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Box;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoxesController(IBoxService boxService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await boxService.GetBoxByIdAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BoxFilterRequest filter)
    {
        var result = await boxService.GetBoxesAsync(filter);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BoxCreateDto dto)
    {
        var result = await boxService.CreateBoxAsync(dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return BadRequest(new { Message = result.Left });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] BoxUpdateDto dto)
    {
        var result = await boxService.UpdateBoxAsync(id, dto);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await boxService.DeleteBoxAsync(id);
        if (result.IsRight)
            return Ok(result.Right);
        else
            return NotFound(new { Message = result.Left });
    }
}
