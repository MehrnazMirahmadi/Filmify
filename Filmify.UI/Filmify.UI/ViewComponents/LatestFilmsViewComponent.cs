using Filmify.Application.Common.Paging;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.ViewComponents;

[ViewComponent(Name = "LatestFilms")]
public class LatestFilmsViewComponent(FilmApiClient api) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(long categoryId, int count = 6)
    {
        var films = await api.GetFilmsByCategoryIdAsync(categoryId, new KeysetPagingRequest { PageSize = count });
        return View(films); // List<FilmDto>
    }
}
