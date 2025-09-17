using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.DTOs.Category;

namespace Filmify.Application.Contracts;

public interface ICategoryService
{
    Task<Either<string, KeysetPagingResult<CategoryDto, long>>> GetCategoriesAsync(CategoryFilterRequest filter, SortOptions? sort = null);

}
