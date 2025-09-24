using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.DTOs.Box;

namespace Filmify.Application.Contracts;

public interface IBoxService
{
    Task<Either<string, BoxDto>> GetBoxByIdAsync(long id);
    Task<Either<string, KeysetPagingResult<BoxDto, long>>> GetBoxesAsync(BoxFilterRequest filter, SortOptions? sort = null);
    Task<Either<string, BoxDto>> CreateBoxAsync(BoxCreateDto dto);
    Task<Either<string, BoxDto>> UpdateBoxAsync(long id, BoxUpdateDto dto);
    Task<Either<string, bool>> DeleteBoxAsync(long id);
}