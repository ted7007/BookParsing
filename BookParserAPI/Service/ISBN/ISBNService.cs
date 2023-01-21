using BookParserAPI.Repository.ISBN;

namespace BookParserAPI.Service.ISBN;

public class ISBNService : IISBNService
{
    private readonly IISBNRepository _repository;

    public ISBNService(IISBNRepository repository)
    {
        _repository = repository;
    }
    
    public Task<IEnumerable<Models.ISBN>> GetAllAsync()
    {
        var result = _repository.GetAllAsync();
        return result;
    }

    public async Task<Models.ISBN> CreateAsync(Models.ISBN isbn)
    {
        var existsISBN = await _repository.GetByValue(isbn.value);
        if (existsISBN != null)
            return existsISBN;
        var result = await _repository.CreateAsync(isbn);
        return result;
    }
}