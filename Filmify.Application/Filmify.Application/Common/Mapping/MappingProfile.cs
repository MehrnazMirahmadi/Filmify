using AutoMapper;
using Filmify.Application.DTOs.Box;
using Filmify.Application.DTOs.Category;
using Filmify.Application.DTOs.Film;
using Filmify.Application.DTOs.Tag;
using Filmify.Domain.Entities;

namespace Filmify.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Film, FilmDto>()
            .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src.FilmId.ToString()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.FilmTags.Select(ft => ft.Tag.TagText)))
            .ForMember(dest => dest.Boxes, opt => opt.MapFrom(src => src.FilmBoxes.Select(fb => fb.Box.BoxName)));

        CreateMap<FilmCreateDto, Film>()
            .ForMember(dest => dest.FilmTags, opt => opt.Ignore())
            .ForMember(dest => dest.FilmBoxes, opt => opt.Ignore());

        CreateMap<FilmUpdateDto, Film>()
             .ForMember(dest => dest.FilmTags, opt => opt.Ignore())  
             .ForMember(dest => dest.FilmBoxes, opt => opt.Ignore()) 
             .ForMember(dest => dest.FilmId, opt => opt.Ignore());   
        CreateMap<Category, CategoryDto>();

        CreateMap<Box, BoxDto>();
        CreateMap<Tag, TagDto>();
    }
}
