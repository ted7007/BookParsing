using AutoMapper;
using BookParserAPI.Dto.Output.Tag;
using BookParserAPI.Models;

namespace BookParserAPI.AutoMapperProfiles;

public class TagMapperProfile  : Profile
{
    public TagMapperProfile()
    {
        CreateMap<Tag, TagDto>();
        CreateMap<TagDto, Tag>();
    }
}