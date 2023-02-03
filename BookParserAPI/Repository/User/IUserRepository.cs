namespace BookParserAPI.Repository.User;

public interface IUserRepository
{
    public Task<Models.User> CreateAsync(Models.User user);

    public Task<Models.User?> GetByLoginAsync(string login);
}