using Filmify.Application.Common.Paging;
using Filmify.Application.DTOs.Box;
using Filmify.Application.DTOs.Category;
using Filmify.Application.DTOs.Film;
using Filmify.Application.DTOs.Tag;
using Filmify.UI.Models;

namespace Filmify.UI.Services;

public class FilmApiClient(HttpClient http)
{

    public async Task<List<FilmDto>> GetFilmsAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<ApiResponse<KeysetPagingResult<FilmDto, long>>>("api/films");
            return response?.Data?.Items?.ToList() ?? new List<FilmDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<FilmDto>();
        }
    }


    public async Task<FilmDto?> GetFilmByIdAsync(long id)
    {
        var response = await http.GetFromJsonAsync<ApiResponse<FilmDto>>($"api/Films/{id}");
        if (response == null || response.Data == null)
            return null;
        return response.Data;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<KeysetPagingResultViewModel<CategoryDto>>("api/category");
            return response?.Items ?? new List<CategoryDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<CategoryDto>();
        }
    }
    public async Task<List<FilmDto>> GetFilmsByCategoryIdAsync(long categoryId, KeysetPagingRequest paging)
    {
        try
        {
            var query = $"api/films/category/{categoryId}?PageSize={paging.PageSize}&LastKey={paging.LastKey}";
            var response = await http.GetFromJsonAsync<KeysetPagingResultViewModel<FilmDto>>(query);
            return response?.Items ?? new List<FilmDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new List<FilmDto>();
        }
    }

    // --- Search films (paged)
    public async Task<KeysetPagingResultViewModel<FilmDto>> SearchFilmsAsync(string searchText, KeysetPagingRequest paging)
    {
        try
        {
            var query = $"api/films/search?SearchText={searchText}&PageSize={paging.PageSize}&LastKey={paging.LastKey}";
            var response = await http.GetFromJsonAsync<KeysetPagingResultViewModel<FilmDto>>(query);
            return response ?? new KeysetPagingResultViewModel<FilmDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new KeysetPagingResultViewModel<FilmDto>();
        }
    }

    public async Task<PagedResult<FilmDto>> GetPagedFilmsAsync(string searchText, int pageNumber, int pageSize)
    {
        var query = $"api/films/paged?SearchText={searchText}&PageNumber={pageNumber}&PageSize={pageSize}";
        var response = await http.GetFromJsonAsync<PagedResult<FilmDto>>(query);
        return response ?? new PagedResult<FilmDto>();
    }
    // --- Create Film
    public async Task<FilmDto?> CreateFilmAsync(FilmCreateDto dto)
    {
        var response = await http.PostAsJsonAsync("api/films", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<FilmDto>();
        return null;
    }

    // --- Update Film
    public async Task<FilmDto?> UpdateFilmAsync(long id, FilmUpdateDto dto)
    {
        var response = await http.PutAsJsonAsync($"api/films/{id}", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<FilmDto>();
        return null;
    }

    // --- Delete Film
    public async Task<bool> DeleteFilmAsync(long id)
    {
        var response = await http.DeleteAsync($"api/films/{id}");
        return response.IsSuccessStatusCode;
    }
    // ----------------- Boxes -----------------

    public async Task<List<BoxDto>> GetAllBoxesAsync()
    {
        var response = await http.GetFromJsonAsync<ApiResponse<KeysetPagingResult<BoxDto, long>>>("api/Boxes");
        return response?.Data?.Items?.ToList() ?? new List<BoxDto>();
    }

    // ----------------- Tags -----------------
    public async Task<List<TagDto>> GetAllTagsAsync()
    {
        var response = await http.GetFromJsonAsync<ApiResponse<KeysetPagingResult<TagDto, long>>>("api/Tags");
        return response?.Data?.Items?.ToList() ?? new List<TagDto>();
    }
    public async Task<long> CreateTagAsync(string tagText)
    {
        var response = await http.PostAsJsonAsync("api/Tags", new { TagText = tagText });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TagDto>>();
            return result!.Data.TagId;
        }
        throw new Exception("Cannot create tag");
    }


}


