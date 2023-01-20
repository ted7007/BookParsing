using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Repository.Book;

public class BookRepository : IBookRepository
{
    private readonly ApplicationContext _context;

    public BookRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<Models.Book> CreateAsync(Models.Book book)
    {
        var res = await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        
        return res.Entity;
    }

    public async Task<IEnumerable<Models.Book>> GetAllAsync()
    {
        return await _context.Books.Include(p => p.Tags).ToListAsync();
    }

    public async Task<Models.Book?> GetAsync(string ISBN)
    {
        return await _context.Books.Include(p => p.Tags).FirstOrDefaultAsync(p => p.ISBN==ISBN);
    }

    public async Task RemoveAsync(string ISBN)
    {
        var productForRemove = await GetAsync(ISBN) ?? throw new InvalidOperationException();
        _context.Books.Remove(productForRemove);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Models.Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }
}