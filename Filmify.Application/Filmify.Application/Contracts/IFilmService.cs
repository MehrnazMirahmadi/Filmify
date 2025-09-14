using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.DTOs.Film;

namespace Filmify.Application.Contracts;

public interface IFilmService
{
    Task<Either<string, FilmDto>> GetFilmByIdAsync(long id);
    Task<Either<string, KeysetPagingResult<FilmDto>>> GetFilmsAsync(FilmFilterRequest filter, SortOptions? sort = null);
    Task<Either<string, FilmDto>> CreateFilmAsync(FilmCreateDto dto);
    Task<Either<string, FilmDto>> UpdateFilmAsync(long id, FilmUpdateDto dto);
    Task<Either<string, bool>> DeleteFilmAsync(long id);
}
