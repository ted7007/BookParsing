using Microsoft.EntityFrameworkCore;

namespace BookParserAPI.Repository.User;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Models.User> CreateAsync(Models.User user)
    {
        var result = await _context.Users.AddAsync(user);
        return result.Entity;
    }

    public Task<Models.User?> GetByLoginAsync(string login)
    {
        var result = _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
        return result;
    }
}