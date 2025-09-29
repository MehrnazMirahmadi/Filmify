using Filmify.Application.Common.Paging;
using Filmify.Application.DTOs.Box;
using Filmify.Application.DTOs.Film;
using Filmify.Application.DTOs.Tag;
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

    public async Task<IActionResult> GetAll(string? searchText, int pageNumber = 1, int pageSize = 4)
    {
        var result = await api.GetPagedFilmsAsync(searchText ?? "", pageNumber, pageSize);
        return View(result);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var film = await api.GetFilmByIdAsync(id);
        if (film == null) return NotFound();
        ViewBag.Boxes = await api.GetAllBoxesAsync();
       ViewBag.Tags = await api.GetAllTagsAsync();
        var allTags = await api.GetAllTagsAsync();
        return PartialView("_EditFilm", new FilmUpdateDto
        {
            FilmId = film.FilmId,
            FilmTitle = film.FilmTitle,
            Duration = film.Duration,
            CoverImage = film.CoverImage,
            Capacity = film.Capacity,
            FileUrl = film.FileUrl,
            BoxIds = film.Boxes?.Select(b => b.BoxId).ToList(),
            TagIds = film.Tags?.Select(t => t.TagId).ToList(),
            AllTags = allTags.ToList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FilmUpdateDto dto)
    {
        var finalTagIds = new List<long>();

        
        if (!string.IsNullOrEmpty(dto.RawTagIds))
        {
            var tags = dto.RawTagIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var tag in tags)
            {
                if (tag.StartsWith("new_"))
                {
                   
                    var tagText = tag.Substring(4).Replace("_", " ");
                    var newTagId = await api.CreateTagAsync(tagText);
                    finalTagIds.Add(newTagId);
                }
                else if (long.TryParse(tag, out var tagId))
                {
                    finalTagIds.Add(tagId);
                }
            }
        }

        dto.TagIds = finalTagIds;

        if (Request.Form.Files.Any())
        {
            var coverFile = Request.Form.Files["coverFile"];
            if (coverFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/covers", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await coverFile.CopyToAsync(stream);

                dto.CoverImage = fileName;
            }
        }
        else
        {
       
            var existingFilm = await api.GetFilmByIdAsync(dto.FilmId);
            dto.CoverImage = existingFilm.CoverImage;
        }

   
        await api.UpdateFilmAsync(dto.FilmId, dto);

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(long id)
    {
        await api.DeleteFilmAsync(id);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var boxes = await api.GetAllBoxesAsync() ?? new List<BoxDto>();
        var tags = await api.GetAllTagsAsync() ?? new List<TagDto>();
        ViewBag.Categories = await api.GetAllCategoriesAsync();
        ViewBag.Boxes = boxes;
        ViewBag.Tags = tags;

        var model = new FilmCreateDto
        {
            AllTags = tags.ToList()
        };

        return PartialView("_CreateFilm", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(FilmCreateDto dto)
    {
        
        var finalTagIds = new List<long>();
        if (!string.IsNullOrEmpty(dto.RawTagIds))
        {
            var tags = dto.RawTagIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var tag in tags)
            {
                if (tag.StartsWith("new_"))
                {
                    var tagText = tag.Substring(4).Replace("_", " ");
                    var newTagId = await api.CreateTagAsync(tagText);
                    finalTagIds.Add(newTagId);
                }
                else if (long.TryParse(tag, out var tagId))
                {
                    finalTagIds.Add(tagId);
                }
            }
        }
        dto.TagIds = finalTagIds;

        // 
        if (Request.Form.Files.Any())
        {
            var coverFile = Request.Form.Files["coverFile"];
            if (coverFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/covers", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await coverFile.CopyToAsync(stream);
                dto.CoverImage = fileName;
            }
        }

        var film = await api.CreateFilmAsync(dto);

        if (film == null)
            return Json(new { success = false, message = "Error creating film" });

        return Json(new { success = true, message = "Film created successfully" });
    }


}
