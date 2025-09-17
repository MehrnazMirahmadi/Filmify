namespace Filmify.UI.Models;

public class KeysetPagingResultViewModel<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public bool HasNextPage { get; set; }
    public long LastKey { get; set; } = 0;
}
