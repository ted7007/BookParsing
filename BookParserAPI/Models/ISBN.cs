using System.ComponentModel.DataAnnotations;

namespace BookParserAPI.Models;


public class ISBN
{
    [Key]
    public string value { get; set; }
}