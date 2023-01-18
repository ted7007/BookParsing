using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Repository.Product;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationContext _context;

    public ProductRepository(ApplicationContext context)
    {
        _context = context;
    }
    public async Task<Models.Book> CreateAsync(Models.Book book)
    {
        var res = await _context.Products.AddAsync(book);
        await _context.SaveChangesAsync();
        
        return res.Entity;
    }

    public async Task<IEnumerable<Models.Book>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Models.Book?> GetAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task RemoveAsync(Guid id)
    {
        var productForRemove = await GetAsync(id) ?? throw new InvalidOperationException();
        _context.Products.Remove(productForRemove);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Models.Book book)
    {
        _context.Products.Update(book);
        await _context.SaveChangesAsync();
    }
}