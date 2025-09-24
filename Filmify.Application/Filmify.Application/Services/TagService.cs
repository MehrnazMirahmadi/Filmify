using AutoMapper;
using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Tag;
using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using System.Linq.Expressions;

namespace Filmify.Application.Services;

public class TagService(IUnitOfWork unitOfWork, IMapper mapper, IPagingService pagingService) : ITagService
{
    public async Task<Either<string, TagDto>> GetTagByIdAsync(long id)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null) return Either<string, TagDto>.Fail("Tag not found");

        return Either<string, TagDto>.Success(mapper.Map<TagDto>(tag));
    }

    public async Task<Either<string, KeysetPagingResult<TagDto, long>>> GetTagsAsync(TagFilterRequest filter, SortOptions? sort = null)
    {
        var query = unitOfWork.Tags.GetAll();

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(t => t.TagText.Contains(filter.SearchText));

        if (sort == null) sort = new SortOptions { SortBy = "TagId", Direction = SortDirection.Desc };
        Expression<Func<Tag, long>> keySelector = t => t.TagId;

        var pagedResult = await pagingService.ToHybridPageAsync(query, keySelector, filter);

        var dtoResult = pagedResult.Items.Select(t => mapper.Map<TagDto>(t)).ToList();

        return Either<string, KeysetPagingResult<TagDto, long>>.Success(
            new KeysetPagingResult<TagDto, long>(dtoResult, pagedResult.HasNextPage, pagedResult.LastKey)
        );
    }

    public async Task<Either<string, TagDto>> CreateTagAsync(TagCreateDto dto)
    {
        var entity = mapper.Map<Tag>(dto);
        await unitOfWork.Tags.AddAsync(entity);
        await unitOfWork.CommitAsync();

        return Either<string, TagDto>.Success(mapper.Map<TagDto>(entity));
    }

    public async Task<Either<string, TagDto>> UpdateTagAsync(long id, TagUpdateDto dto)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null) return Either<string, TagDto>.Fail("Tag not found");

        mapper.Map(dto, tag);
        await unitOfWork.Tags.UpdateAsync(tag);
        await unitOfWork.CommitAsync();

        return Either<string, TagDto>.Success(mapper.Map<TagDto>(tag));
    }

    public async Task<Either<string, bool>> DeleteTagAsync(long id)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id);
        if (tag == null) return Either<string, bool>.Fail("Tag not found");

        await unitOfWork.Tags.DeleteAsync(id);
        await unitOfWork.CommitAsync();

        return Either<string, bool>.Success(true);
    }

    
}
