using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs;
using Filmify.Application.DTOs.Category;
using Filmify.Application.DTOs.Film;
using Filmify.Application.DTOs.Tag;
using Filmify.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController (ICategoryService categoryService): ControllerBase
    {
        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CategoryFilterRequest filter)
        {

            var sortOptions = new SortOptions
            {
                SortBy = "CategoryId",
                Direction = filter.Direction
            };

            var result = await categoryService.GetCategoriesAsync(filter, sortOptions);
            if (result.IsRight)
                return Ok(result.Right);
            else
                return NotFound(new { Message = result.Left });
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategoryFilterRequest filter)
        {

            var sortOptions = new SortOptions
            {
                SortBy = "CategoryId",
                Direction = filter.Direction
            };

            var result = await categoryService.GetCategoriesAsync(filter, sortOptions);

            return result.IsRight
                ? Ok(ApiResponse<KeysetPagingResult<CategoryDto, long>>.Ok(result.Right))
                : NotFound(ApiResponse<KeysetPagingResult<CategoryDto, long>>.Fail(result.Left));
        }
    }
}
