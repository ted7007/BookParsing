using BookParserAPI.Models;

namespace BookParserAPI.Repository.Product;

public interface IProductRepository
{ 
    Task<Models.Book> CreateAsync(Models.Book book);
    
    Task<IEnumerable<Models.Book>> GetAllAsync();

    Task<Models.Book?> GetAsync(Guid id);

    Task RemoveAsync(Guid id);

    Task UpdateAsync(Models.Book book);
}