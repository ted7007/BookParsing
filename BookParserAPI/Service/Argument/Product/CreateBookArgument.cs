﻿namespace BookParserAPI.Service.Argument.Product;

public class CreateBookArgument
{
    public string Name { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public string Year { get; set; }
    
    //todo разобраться с картинкой

    public string ISBN { get; set; }

    public int PagesCount { get; set; }

    public string Genre { get; set; }

    public List<string> Tags { get; set; }
}