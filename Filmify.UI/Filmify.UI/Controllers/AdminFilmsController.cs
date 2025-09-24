using Filmify.Application.DTOs.Film;
using Filmify.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminFilmsController(FilmApiClient api) : Controller
{
    public async Task<IActionResult> Index()
    {
        var films = await api.GetFilmsAsync();
        return View(films);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new FilmCreateDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(FilmCreateDto dto, IFormFile? coverFile)
    {
        if (coverFile != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await coverFile.CopyToAsync(stream);

            dto.CoverImage = "/uploads/" + fileName;
        }

        await api.CreateFilmAsync(dto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var film = await api.GetFilmByIdAsync(id);
        if (film == null) return NotFound();
        ViewBag.Boxes = await api.GetAllBoxesAsync(); // List<BoxDto>
        ViewBag.Tags = await api.GetAllTagsAsync();   // List<TagDto>
        return View(new FilmUpdateDto
        {
            FilmTitle = film.FilmTitle,
            Duration = film.Duration,
            CoverImage = film.CoverImage,
            Capacity = film.Capacity,
            FileUrl = film.FileUrl,
            BoxIds = film.Boxes?.Select(b => b.BoxId).ToList(),
            TagIds = film.Tags?.Select(t => t.TagId).ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long id, FilmUpdateDto dto, IFormFile? coverFile)
    {
        if (coverFile != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await coverFile.CopyToAsync(stream);

            dto.CoverImage = "/uploads/" + fileName;
        }

        await api.UpdateFilmAsync(id, dto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(long id)
    {
        await api.DeleteFilmAsync(id);
        return RedirectToAction("Index");
    }
}
