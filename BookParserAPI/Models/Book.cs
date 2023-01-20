using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookParserAPI.Models;

public class Book
{
    public string Name { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public string Year { get; set; }
    
    //todo разобраться с картинкой
    [Key]
    public string ISBN { get; set; }

    public int PagesCount { get; set; }

    public string Genre { get; set; }
    
    public ICollection<Tag> Tags { get; set; }
}