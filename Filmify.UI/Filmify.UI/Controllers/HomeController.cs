using Filmify.Application.Common.Paging;
using Filmify.UI.Models;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Filmify.UI.Controllers
{
    public class HomeController(FilmApiClient api) : Controller
    {
      
        public async Task<IActionResult> Index()
        {
            var paging = new KeysetPagingRequest { PageSize = 4, LastKey = "0"};
            var films = await api.SearchFilmsAsync("", paging); 
            return View(films);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
