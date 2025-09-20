using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.DTOs.Film;

namespace Filmify.Application.Contracts;

public interface IFilmService
{
    Task<Either<string, FilmDto>> GetFilmByIdAsync(long id);
    Task<Either<string, KeysetPagingResult<FilmDto, long>>> GetFilmsAsync(FilmFilterRequest filter, SortOptions? sort = null);
    Task<Either<string, FilmDto>> CreateFilmAsync(FilmCreateDto dto);
    Task<Either<string, FilmDto>> UpdateFilmAsync(long id, FilmUpdateDto dto);
    Task<Either<string, bool>> DeleteFilmAsync(long id);
    Task<Either<string, KeysetPagingResult<FilmDto, long>>> GetFilmsByCategoryAsync(long categoryId, KeysetPagingRequest paging);
    Task<Either<string, List<FilmDto>>> GetLatestFilmsByCategoryAsync(long categoryId, int count = 6);
    Task<KeysetPagingResult<FilmDto, long>> SearchFilmsAsync(FilmSearchRequest request);
    Task<PagedResult<FilmDto>> GetPagedFilmsAsync(string searchText, int pageNumber, int pageSize);
}
