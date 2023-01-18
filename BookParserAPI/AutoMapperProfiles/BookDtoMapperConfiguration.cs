using AutoMapper;
using BookParserAPI.Dto.Output.Product;
using BookParserAPI.Models;

namespace BookParserAPI.AutoMapperProfiles;

public class BookDtoMapperConfiguration : Profile
{
    public BookDtoMapperConfiguration()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();
        
        
    }
}