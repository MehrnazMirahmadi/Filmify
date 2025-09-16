namespace Filmify.Application.Common.Paging;

public class KeysetPagingResult<T, TKey>
{
    public IReadOnlyList<T> Items { get; }
    public bool HasNextPage { get; }
    public TKey LastKey { get; }

    public KeysetPagingResult(IReadOnlyList<T> items, bool hasNextPage, TKey lastKey)
    {
        Items = items;
        HasNextPage = hasNextPage;
        LastKey = lastKey;
    }
}