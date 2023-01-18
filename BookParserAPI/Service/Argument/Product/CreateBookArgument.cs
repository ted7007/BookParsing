namespace BookParserAPI.Service.Argument.Product;

public class CreateBookArgument
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }
}