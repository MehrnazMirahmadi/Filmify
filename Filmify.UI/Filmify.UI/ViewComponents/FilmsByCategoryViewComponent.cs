using Filmify.Application.Common.Paging;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.ViewComponents;

[ViewComponent(Name = "FilmsByCategory")]
public class FilmsByCategoryViewComponent(FilmApiClient api) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(long categoryId, int pageSize = 5)
    {
        var paging = new KeysetPagingRequest { PageSize = pageSize };
        var films = await api.GetFilmsByCategoryIdAsync(categoryId, paging);
        return View(films);
    }
}
