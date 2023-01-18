using BookParserAPI.Models;
using BookParserAPI.Service.Argument.Product;

namespace BookParserAPI.Service;

public interface IBookService
{
    Task<Book> CreateAsync(CreateBookArgument argument);

    Task UpdateAsync(UpdateBookArgument argument);

    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetAsync(Guid id);

}