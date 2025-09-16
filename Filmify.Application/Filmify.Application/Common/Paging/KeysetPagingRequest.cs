using Filmify.Application.Common.Sorting;

namespace Filmify.Application.Common.Paging;

public class KeysetPagingRequest
{
    
    public int PageSize { get; set; } = 10;

    /// <summary>Keyset Pagination</summary>
    public string? LastKey { get; set; }


    public SortDirection Direction { get; set; } = SortDirection.Desc;

    /// <summary>(Offset Pagination)</summary>
    public int? PageNumber { get; set; } = null;
}
