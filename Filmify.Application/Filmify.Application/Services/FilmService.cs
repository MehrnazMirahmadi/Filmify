using AutoMapper;
using Filmify.Application.Common;
using Filmify.Application.Common.Paging;
using Filmify.Application.Common.Sorting;
using Filmify.Application.Contracts;
using Filmify.Application.DTOs.Box;
using Filmify.Application.DTOs.Film;
using Filmify.Application.DTOs.Tag;
using Filmify.Domain.Contracts.Interfaces;
using Filmify.Domain.Entities;
using System.Linq.Expressions;

namespace Filmify.Application.Services;

public class FilmService(IUnitOfWork unitOfWork, IMapper mapper, IPagingService pagingService) : IFilmService
{

    public async Task<Either<string, FilmDto>> GetFilmByIdAsync(long id)
    {
        var film = await unitOfWork.Films.GetFilmWithRelationsAsync(id);
        if (film == null) return Either<string, FilmDto>.Fail("Film not found");

        var dto = mapper.Map<FilmDto>(film);
        return Either<string, FilmDto>.Success(dto);
    }
    public async Task<Either<string, KeysetPagingResult<FilmDto, long>>> GetFilmsAsync(
     FilmFilterRequest filter,
     SortOptions? sort = null)
    {
        var query = unitOfWork.Films.QueryWithRelations();

        // --- Filtering
        if (!string.IsNullOrWhiteSpace(filter.FilmTitle))
            query = query.Where(f => f.FilmTitle.Contains(filter.FilmTitle));

        if (!string.IsNullOrWhiteSpace(filter.BoxName))
            query = query.Where(f => f.FilmBoxes.Any(b => b.Box.BoxName.Contains(filter.BoxName)));

        if (!string.IsNullOrWhiteSpace(filter.TagName))
            query = query.Where(f => f.FilmTags.Any(t => t.Tag.TagText.Contains(filter.TagName)));

        if (filter.FromRegDate.HasValue)
            query = query.Where(f => f.RegDate >= filter.FromRegDate.Value);

        if (filter.ToRegDate.HasValue)
            query = query.Where(f => f.RegDate <= filter.ToRegDate.Value);

        // --- Sorting & KeySelector
        if (sort == null) sort = new SortOptions { SortBy = "FilmId", Direction = SortDirection.Desc };
        Expression<Func<Film, long>> keySelector = f => f.FilmId;

        var pagedResult = await pagingService.ToHybridPageAsync(query, keySelector, filter);

        // Map to DTO after paging
        var dtoResult = pagedResult.Items.Select(f => mapper.Map<FilmDto>(f)).ToList();

        var result = new KeysetPagingResult<FilmDto, long>(
            dtoResult,
            pagedResult.HasNextPage,
            pagedResult.LastKey
        );

        return Either<string, KeysetPagingResult<FilmDto, long>>.Success(result);
    }

    public async Task<Either<string, FilmDto>> CreateFilmAsync(FilmCreateDto dto)
    {
        if (await unitOfWork.Films.CheckDuplicateTitleAsync(dto.FilmTitle))
            return Either<string, FilmDto>.Fail("Film with the same title already exists");

        var film = mapper.Map<Film>(dto);


        if (dto.BoxIds != null)
            film.FilmBoxes = dto.BoxIds.Select(id => new FilmBox { BoxId = id }).ToList();

        if (dto.TagIds != null)
            film.FilmTags = dto.TagIds.Select(id => new FilmTag { TagId = id }).ToList();

        await unitOfWork.Films.AddAsync(film);
        await unitOfWork.CommitAsync();

        var filmWithRelations = await unitOfWork.Films.GetFilmWithRelationsAsync(film.FilmId);
        var filmDto = mapper.Map<FilmDto>(filmWithRelations);

        return Either<string, FilmDto>.Success(filmDto);

    }

    public async Task<Either<string, FilmDto>> UpdateFilmAsync(long id, FilmUpdateDto dto)
    {
        var film = await unitOfWork.Films.GetFilmWithRelationsAsync(id);
        if (film == null) return Either<string, FilmDto>.Fail("Film not found");

        if (film.FilmTitle != dto.FilmTitle &&
           await unitOfWork.Films.CheckDuplicateTitleAsync(dto.FilmTitle))
            return Either<string, FilmDto>.Fail("Film with the same title already exists");


        mapper.Map(dto, film);


        if (dto.BoxIds != null)
        {
            var newBoxIds = new HashSet<long>(dto.BoxIds);
            var existingBoxIds = new HashSet<long>(film.FilmBoxes.Select(fb => fb.BoxId));


            var toRemove = film.FilmBoxes.Where(fb => !newBoxIds.Contains(fb.BoxId)).ToList();
            foreach (var r in toRemove) film.FilmBoxes.Remove(r);


            var toAdd = newBoxIds.Except(existingBoxIds)
                .Select(bid => new FilmBox { FilmId = film.FilmId, BoxId = bid });
            foreach (var a in toAdd) film.FilmBoxes.Add(a);
        }

        if (dto.TagIds != null)
        {
            var newTagIds = new HashSet<long>(dto.TagIds);
            var existingTagIds = new HashSet<long>(film.FilmTags.Select(ft => ft.TagId));


            var toRemove = film.FilmTags.Where(ft => !newTagIds.Contains(ft.TagId)).ToList();
            foreach (var r in toRemove) film.FilmTags.Remove(r);


            var toAdd = newTagIds.Except(existingTagIds)
                .Select(tid => new FilmTag { FilmId = film.FilmId, TagId = tid });
            foreach (var a in toAdd) film.FilmTags.Add(a);
        }


        await unitOfWork.Films.UpdateAsync(film);
        await unitOfWork.CommitAsync();


        film = await unitOfWork.Films.GetFilmWithRelationsAsync(film.FilmId);

        var filmDto = mapper.Map<FilmDto>(film);

        return Either<string, FilmDto>.Success(filmDto);
    }


    public async Task<Either<string, bool>> DeleteFilmAsync(long id)
    {
        var film = await unitOfWork.Films.GetByIdAsync(id);
        if (film == null) return Either<string, bool>.Fail("Film not found");

        await unitOfWork.Films.DeleteAsync(id);
        await unitOfWork.CommitAsync();

        return Either<string, bool>.Success(true);
    }
    public async Task<Either<string, KeysetPagingResult<FilmDto, long>>> GetFilmsByCategoryAsync(
       long categoryId,
       KeysetPagingRequest paging)
    {

        var query = unitOfWork.Films.QueryWithRelations()
            .Where(f => f.CategoryId == categoryId)
            .OrderByDescending(f => f.RegDate);


        var pagedResult = await pagingService.ToHybridPageAsync(query, f => f.FilmId, paging);


        var dtoResult = pagedResult.Items.Select(f => new FilmDto
        {
            FilmId = f.FilmId,
            FilmTitle = f.FilmTitle,
            CoverImage = f.CoverImage,
            RegDate = f.RegDate,
            CategoryName = f.Category.Name,
            LikeCount = f.LikeCount,
            ViewCount = f.ViewCount,
            FilmScore = f.FilmScore,
            Tags = f.FilmTags?.Select(ft => new TagDto
            {
                TagId = ft.TagId,
                TagText = ft.Tag.TagText
            }).ToList(),
            Boxes = f.FilmBoxes?.Select(fb => new BoxDto
            {
                BoxId = fb.BoxId,
                BoxName = fb.Box.BoxName
            }).ToList()
        }).ToList();


        var result = new KeysetPagingResult<FilmDto, long>(
            dtoResult,
            pagedResult.HasNextPage,
            pagedResult.LastKey
        );

        return Either<string, KeysetPagingResult<FilmDto, long>>.Success(result);
    }
    public async Task<Either<string, List<FilmDto>>> GetLatestFilmsByCategoryAsync(long categoryId, int count = 6)
    {
        try
        {
            var query = unitOfWork.Films.QueryWithRelations()
                .Where(f => f.CategoryId == categoryId)
                .OrderByDescending(f => f.ReleaseDate)
                .Take(count);

            var films = unitOfWork.Films.QueryWithRelations()
                     .Where(f => f.CategoryId == categoryId)
                     .OrderByDescending(f => f.ReleaseDate)
                     .Take(6)
                     .AsEnumerable()
                     .Select(f => new FilmDto
                     {
                         FilmId = f.FilmId,
                         FilmTitle = f.FilmTitle,
                         CoverImage = f.CoverImage,
                         RegDate = f.RegDate,
                         CategoryName = f.Category.Name,
                         LikeCount = f.LikeCount,
                         ViewCount = f.ViewCount,
                         ReleaseYear = f.ReleaseDate?.Year
                     })
                     .ToList();





            return Either<string, List<FilmDto>>.Success(films);
        }
        catch (Exception ex)
        {
            return Either<string, List<FilmDto>>.Fail($"Error: {ex.Message}");
        }
    }

    public async Task<KeysetPagingResult<FilmDto, long>> SearchFilmsAsync(FilmSearchRequest request)
    {
        long? lastKey = null;
        if (!string.IsNullOrEmpty(request.LastKey) && long.TryParse(request.LastKey, out var parsed))
            lastKey = parsed;

        var films = await unitOfWork.Films.SearchAsync(
            key: request.SearchText,
            lastKey: lastKey,
            pageSize: request.PageSize
        );

        var filmDtos = films.Select(f => new FilmDto
        {
            FilmId = f.FilmId,
            FilmTitle = f.FilmTitle,
            CategoryName = f.Category?.Name,
            Tags = f.FilmTags?.Select(ft => new TagDto
            {
                TagId = ft.TagId,
                TagText = ft.Tag.TagText
            }).ToList(),
            Boxes = f.FilmBoxes?.Select(fb => new BoxDto
            {
                BoxId = fb.BoxId,
                BoxName = fb.Box.BoxName
            }).ToList(),
            CoverImage = f.CoverImage,
            RegDate = f.RegDate,
            LikeCount = f.LikeCount,
            ViewCount = f.ViewCount,
            FilmScore = f.FilmScore,
        }).ToList();

        bool hasNext = filmDtos.Count > request.PageSize;
        if (hasNext) filmDtos.RemoveAt(filmDtos.Count - 1);

        long lastKeyResult = filmDtos.Any() ? filmDtos.Last().FilmId : 0;

        return new KeysetPagingResult<FilmDto, long>(filmDtos, hasNext, lastKeyResult);
    }

    public async Task<PagedResult<FilmDto>> GetPagedFilmsAsync(string searchText, int pageNumber, int pageSize)
    {
        var films = await unitOfWork.Films.SearchAsync(searchText, pageNumber, pageSize);
        var totalCount = await unitOfWork.Films.CountAsync(searchText);

        var items = films.Select(f => new FilmDto
        {
            FilmId = f.FilmId,
            FilmTitle = f.FilmTitle,
            CategoryName = f.Category?.Name,
            Tags = f.FilmTags?.Select(ft => new TagDto
            {
                TagId = ft.TagId,
                TagText = ft.Tag.TagText
            }).ToList(),
            Boxes = f.FilmBoxes?.Select(fb => new BoxDto
            {
                BoxId = fb.BoxId,
                BoxName = fb.Box.BoxName
            }).ToList(),
            CoverImage = f.CoverImage,
            RegDate = f.RegDate,
            LikeCount = f.LikeCount,
            ViewCount = f.ViewCount,
            FilmScore = f.FilmScore
        }).ToList();

        return new PagedResult<FilmDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }


}
