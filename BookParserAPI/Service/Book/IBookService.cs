using BookParserAPI.Service.Argument.Book;

namespace BookParserAPI.Service.Book;

public interface IBookService
{
    Task<Models.Book> CreateAsync(CreateBookArgument argument);

    Task UpdateAsync(UpdateBookArgument argument);

    Task<IEnumerable<Models.Book>> GetAllAsync();

    Task<Models.Book?> GetAsync(string ISBN);

}