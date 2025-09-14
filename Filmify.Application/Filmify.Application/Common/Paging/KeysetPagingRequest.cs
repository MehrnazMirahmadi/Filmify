using Filmify.Application.Common.Sorting;

namespace Filmify.Application.Common.Paging;

public class KeysetPagingRequest
{
    /// <summary>تعداد آیتم در هر صفحه</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>آخرین مقدار کلید برای Keyset Pagination</summary>
    public string? LastKey { get; set; }

    /// <summary>جهت مرتب‌سازی</summary>
    public SortDirection Direction { get; set; } = SortDirection.Desc;

    /// <summary>شماره صفحه برای دسترسی مستقیم (Offset Pagination)</summary>
    public int? PageNumber { get; set; } = null;
}
