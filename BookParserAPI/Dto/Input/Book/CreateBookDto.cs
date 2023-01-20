using System.ComponentModel.DataAnnotations;
using BookParserAPI.Dto.Output.Tag;
using BookParserAPI.Models;

namespace BookParserAPI.Dto.Input.Book;

public class CreateBookDto
{
    public string Name { get; set; }
    
    public string Author { get; set; }

    public string Description { get; set; }

    public string Year { get; set; }
    
    //todo разобраться с картинкой
    
    public string ISBN { get; set; }

    public int PagesCount { get; set; }

    public string Genre { get; set; }

    public ICollection<TagDto> Tags { get; set; }
}