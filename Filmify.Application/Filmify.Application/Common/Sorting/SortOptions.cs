namespace Filmify.Application.Common.Sorting;

public class SortOptions
{
    public string SortBy { get; set; } = "Id";
    public SortDirection Direction { get; set; } = SortDirection.Asc;
}
