using Filmify.Application.Common.Paging;
using Filmify.Application.DTOs.Category;
using Filmify.Application.DTOs.Film;
using Filmify.UI.Models;
using static System.Net.WebRequestMethods;

namespace Filmify.UI.Services;

public class FilmApiClient(HttpClient http)
{
   
    public async Task<List<FilmDto>> GetFilmsAsync()
    {
        try
        {
            var response = await http.GetFromJsonAsync<KeysetPagingResultViewModel<FilmDto>>("api/films");
            return response?.Items ?? new List<FilmDto>();
        }
        catch (HttpRequestException httpEx)
        {
            // Log http errors
            Console.WriteLine(httpEx);

            return new List<FilmDto>();
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);

            return new List<FilmDto>();
        }
    }


    public async Task<FilmDto> GetFilmByIdAsync(long id)
    {
        var response = await http.GetFromJsonAsync<ApiResponse<FilmDto>>($"api/films/{id}");
        return response?.Data!;
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
  

}


