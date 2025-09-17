using Filmify.Application.Common.Paging;
using Filmify.UI.Models;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.ViewComponents;

[ViewComponent(Name = "FilmsByCategories")]
public class FilmsByCategoriesViewComponent(FilmApiClient api) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {

        var categories = await api.GetCategoriesAsync(); // List<CategoryDto>
        var result = new List<CategoryWithFilmsViewModel>();

        foreach (var category in categories)
        {
            var films = await api.GetFilmsByCategoryIdAsync(category.CategoryId, new KeysetPagingRequest { PageSize = 6 });
            result.Add(new CategoryWithFilmsViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                Films = films
            });
        }

        return View(result);
    }
}
