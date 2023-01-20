using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Models;

public class Tag
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public ICollection<Book>? Books { get; set; }
   

}