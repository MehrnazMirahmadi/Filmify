using AutoMapper;
using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Category;
using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using System.Linq.Expressions;

namespace Filmify.Application.Services;

public class CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IPagingService pagingService) : ICategoryService
{
    public async Task<Either<string, KeysetPagingResult<CategoryDto, long>>> GetCategoriesAsync(CategoryFilterRequest filter, SortOptions? sort = null)
    {
        var query = unitOfWork.Categories.GetAll();

        if (!string.IsNullOrWhiteSpace(filter.CategoryName))
        {
            query = query.Where(c => c.Name.Contains(filter.CategoryName));
        }
        // --- Sorting & KeySelector
        if (sort == null) sort = new SortOptions { SortBy = "CategoryId", Direction = SortDirection.Desc };
        Expression<Func<Category, long>> keySelector = f => f.CategoryId;
        var pagedResult = await pagingService.ToHybridPageAsync(query, keySelector, filter);

        // Map to DTO after paging
        var dtoResult = pagedResult.Items.Select(f => mapper.Map<CategoryDto>(f)).ToList();

        var result = new KeysetPagingResult<CategoryDto, long>(
            dtoResult,
            pagedResult.HasNextPage,
            pagedResult.LastKey
        );

        return Either<string, KeysetPagingResult<CategoryDto, long>>.Success(result);
    }
}
