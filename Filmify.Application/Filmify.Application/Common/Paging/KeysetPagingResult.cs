namespace Filmify.Application.Common.Paging;

public class KeysetPagingResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public bool HasNextPage { get; }

    public KeysetPagingResult(IReadOnlyList<T> items, bool hasNextPage)
    {
        Items = items;
        HasNextPage = hasNextPage;
    }
}
