using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.DTOs.Tag;

namespace Filmify.Application.Contracts;

public interface ITagService
{
    Task<Either<string, TagDto>> GetTagByIdAsync(long id);
    Task<Either<string, KeysetPagingResult<TagDto, long>>> GetTagsAsync(TagFilterRequest filter, SortOptions? sort = null);
    Task<Either<string, TagDto>> CreateTagAsync(TagCreateDto dto);
    Task<Either<string, TagDto>> UpdateTagAsync(long id, TagUpdateDto dto);
    Task<Either<string, bool>> DeleteTagAsync(long id);
}