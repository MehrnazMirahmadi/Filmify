namespace Filmify.UI.Models;

public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Search { get; set; }
    public string ActionName { get; set; } = "";
    public string ControllerName { get; set; } = "";
}
