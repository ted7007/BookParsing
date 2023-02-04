using BookParserAPI.Service.Argument.User;

namespace BookParserAPI.Service.User;

public interface IUserService
{
    public Task<Models.User> CreateAsync(CreateUserArgument argument);

    public Task<Models.User?> GetByLoginAsync(string login);
}