using Filmify.Application.Common.Paging;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.ViewComponents;

[ViewComponent(Name = "FilmSearch")]
public class FilmSearchViewComponent(FilmApiClient api) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string? searchText = "", int pageSize = 10, string? lastKey = null)
    {
        var paging = new KeysetPagingRequest
        {
            PageSize = pageSize,
            LastKey = lastKey
        };

        var result = await api.SearchFilmsAsync(searchText ?? "", paging);

        return View(result); 
    }
}
