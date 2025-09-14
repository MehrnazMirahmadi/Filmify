using Filmify.Application.DTOs.Film;
using Filmify.UI.Models;

namespace Filmify.UI.Services;

public class FilmApiClient
{
    private readonly HttpClient _http;

    public FilmApiClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:7053/"); 
    }

    public async Task<List<FilmDto>> GetFilmsAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<KeysetPagingResult<FilmDto>>("api/films");
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



    //public async Task<FilmDto> GetFilmByIdAsync(long id)
    //{
    //    var response = await _http.GetFromJsonAsync<ApiResponse<FilmDto>>($"api/films/{id}");
    //    return response?.Data!;
    //}

}


