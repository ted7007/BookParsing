namespace BookParserAPI.Repository.ISBN;

public interface IISBNRepository
{
    public Task<IEnumerable<Models.ISBN>> GetAllAsync();

    public Task<Models.ISBN> CreateAsync(Models.ISBN isbn);

    public Task<Models.ISBN?> GetByValue(string value);
}