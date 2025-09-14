namespace Filmify.UI.Models;

public class KeysetPagingResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public bool HasNextPage { get; set; }
}
