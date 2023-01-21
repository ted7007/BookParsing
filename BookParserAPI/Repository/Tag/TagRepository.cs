using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Repository.Tag;

public class TagRepository : ITagRepository
{
    private readonly ApplicationContext _context;


    public TagRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<Models.Tag?> GetByNameAsync(string name)
    {
        var result = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        return result;
    }

    public async Task<Models.Tag> CreateAsync(Models.Tag tag)
    {
        var result = await _context.Tags.AddAsync(tag);
        return result.Entity;
    }
}