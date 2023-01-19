using AutoMapper;
using BookParserAPI.Dto.Input.Book;
using BookParserAPI.Service.Argument.Book;

namespace BookParserAPI.AutoMapperProfiles;

public class CreateBookMapperConfiguration : Profile
{
    public CreateBookMapperConfiguration()
    {
        CreateMap<CreateBookDto, CreateBookArgument>();
        CreateMap<CreateBookArgument, CreateBookDto>();  
    }
    
}