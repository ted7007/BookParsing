using AutoMapper;
using BookParserAPI.Dto.Output.Book;
using BookParserAPI.Models;

namespace BookParserAPI.AutoMapperProfiles;

public class BookDtoMapperConfiguration : Profile
{
    public BookDtoMapperConfiguration()
    {
        AllowNullCollections = true;
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();
        
        
    }
}