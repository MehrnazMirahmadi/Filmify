using Filmify.Application.DTOs.Film;

namespace Filmify.UI.Models;

public class CategoryWithFilmsViewModel
{
    public long CategoryId { get; set; }
    public string CategoryName { get; set; }
    public List<FilmDto> Films { get; set; } = new();
}
