namespace BookParserAPI.Dto.Output.Product;

public class BookDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }
}