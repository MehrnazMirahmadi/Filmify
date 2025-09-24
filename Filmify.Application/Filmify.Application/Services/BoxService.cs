using AutoMapper;
using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Box;
using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using System.Linq.Expressions;

namespace Filmify.Application.Services;

public class BoxService(IUnitOfWork unitOfWork, IMapper mapper, IPagingService pagingService) : IBoxService
{
    public async Task<Either<string, BoxDto>> GetBoxByIdAsync(long id)
    {
        var box = await unitOfWork.Boxes.GetByIdAsync(id);
        if (box == null) return Either<string, BoxDto>.Fail("Box not found");

        return Either<string, BoxDto>.Success(mapper.Map<BoxDto>(box));
    }

    public async Task<Either<string, KeysetPagingResult<BoxDto, long>>> GetBoxesAsync(BoxFilterRequest filter, SortOptions? sort = null)
    {
        var query = unitOfWork.Boxes.GetAll();

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(b => b.BoxName.Contains(filter.SearchText));

        if (sort == null) sort = new SortOptions { SortBy = "BoxId", Direction = SortDirection.Desc };
        Expression<Func<Box, long>> keySelector = b => b.BoxId;

        var pagedResult = await pagingService.ToHybridPageAsync(query, keySelector, filter);

        var dtoResult = pagedResult.Items.Select(b => mapper.Map<BoxDto>(b)).ToList();

        return Either<string, KeysetPagingResult<BoxDto, long>>.Success(
            new KeysetPagingResult<BoxDto, long>(dtoResult, pagedResult.HasNextPage, pagedResult.LastKey)
        );
    }

    public async Task<Either<string, BoxDto>> CreateBoxAsync(BoxCreateDto dto)
    {
        var entity = mapper.Map<Box>(dto);
        await unitOfWork.Boxes.AddAsync(entity);
        await unitOfWork.CommitAsync();
        return Either<string, BoxDto>.Success(mapper.Map<BoxDto>(entity));
    }

    public async Task<Either<string, BoxDto>> UpdateBoxAsync(long id, BoxUpdateDto dto)
    {
        var box = await unitOfWork.Boxes.GetByIdAsync(id);
        if (box == null) return Either<string, BoxDto>.Fail("Box not found");

        mapper.Map(dto, box);
        await unitOfWork.Boxes.UpdateAsync(box);
        await unitOfWork.CommitAsync();

        return Either<string, BoxDto>.Success(mapper.Map<BoxDto>(box));
    }

    public async Task<Either<string, bool>> DeleteBoxAsync(long id)
    {
        var box = await unitOfWork.Boxes.GetByIdAsync(id);
        if (box == null) return Either<string, bool>.Fail("Box not found");

        await unitOfWork.Boxes.DeleteAsync(id);
        await unitOfWork.CommitAsync();

        return Either<string, bool>.Success(true);
    }
}