using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers;

public class FilmsController : Controller
{
    private readonly FilmApiClient _api;

    public FilmsController(FilmApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var films = await _api.GetFilmsAsync();
        return View(films);
    }

    //public async Task<IActionResult> Details(long id)
    //{
    //    var film = await _api.GetFilmByIdAsync(id);
    //    if (film == null) return NotFound();
    //    return View(film);
    //}
}
