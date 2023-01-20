using AutoMapper;
using BookParserAPI.Dto.Input.Book;
using BookParserAPI.Models;
using BookParserAPI.Service.Argument.Book;

namespace BookParserAPI.AutoMapperProfiles;

public class CreateBookMapperConfiguration : Profile
{
    public CreateBookMapperConfiguration()
    {
        
        AllowNullCollections = true;
        CreateMap<CreateBookDto, CreateBookArgument>();  
        CreateMap<CreateBookArgument, Book>(); 
        
    }
    
}