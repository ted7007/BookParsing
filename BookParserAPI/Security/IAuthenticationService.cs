using BookParserAPI.Dto.Input.User;

namespace BookParserAPI.Security;

public interface IAuthenticationService
{
    public Task<string?> GetTokenAsync(LoginModel model);
}