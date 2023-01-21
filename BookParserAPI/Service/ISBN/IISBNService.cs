namespace BookParserAPI.Service.ISBN;

public interface IISBNService
{
    public Task<IEnumerable<Models.ISBN>> GetAllAsync();

    public Task<Models.ISBN> CreateAsync(Models.ISBN isbn);
}