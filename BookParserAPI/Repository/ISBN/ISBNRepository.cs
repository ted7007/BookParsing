using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Repository.ISBN;

public class ISBNRepository : IISBNRepository
{
    private readonly ApplicationContext _context;

    public ISBNRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Models.ISBN>> GetAllAsync()
    {
        var result = await _context.ISBNs.ToListAsync();
        return result;
    }

    public async Task<Models.ISBN> CreateAsync(Models.ISBN isbn)
    {
        var result = await _context.ISBNs.AddAsync(isbn);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Models.ISBN?> GetByValue(string value)
    {
        var result = await _context.ISBNs.FirstOrDefaultAsync(i => i.value == value);
        return result;
    }
}