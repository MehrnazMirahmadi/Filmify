using Filmify.Application.Common.Paging;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            PageSize = 6,
            LastKey = lastKey
        };

        var result = await api.SearchFilmsAsync(searchText, paging);

        return PartialView("_FilmSearchResults", result);
    }
   
        public async Task<IActionResult> GetAll(string? searchText, int pageNumber = 1, int pageSize = 6)
        {
            var result = await api.GetPagedFilmsAsync(searchText ?? "", pageNumber, pageSize);
            return View(result);
        }
    

}
