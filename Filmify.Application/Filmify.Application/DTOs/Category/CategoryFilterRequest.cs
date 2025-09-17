using Filmify.Application.Common.Paging;

namespace Filmify.Application.DTOs.Category;

public class CategoryFilterRequest : KeysetPagingRequest
{
    public string? CategoryName { get; set; }

}
