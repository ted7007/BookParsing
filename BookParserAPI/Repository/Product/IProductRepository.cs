using BookParserAPI.Models;

namespace BookParserAPI.Repository.Book;

public interface IBookRepository
{ 
    Task<Models.Book> CreateAsync(Models.Book book);
    
    Task<IEnumerable<Models.Book>> GetAllAsync();

    Task<Models.Book?> GetAsync(string isbn);

    Task RemoveAsync(string isbn);

    Task UpdateAsync(Models.Book book);
}