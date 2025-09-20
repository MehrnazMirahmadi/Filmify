using Filmify.Application.Common.Paging;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers;

public class FilmsController(FilmApiClient api) : Controller
{
    public async Task<IActionResult> Index()
    {
        var films = await api.GetFilmsAsync();
        return View(films);
    }

    public async Task<IActionResult> Details(long id)
    {
        var film = await api.GetFilmByIdAsync(id);
        if (film == null) return NotFound();
        return View(film);
    }

    public async Task<IActionResult> Search(string searchText, string lastKey = "")
    {
        var paging = new KeysetPagingRequest
        {
            PageSize = 10,
            LastKey = lastKey
        };

        var result = await api.SearchFilmsAsync(searchText, paging);

        return PartialView("_FilmSearchResults", result);
    }

}
