using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookParserAPI.Config;
using BookParserAPI.Dto.Input.User;
using BookParserAPI.Models;
using BookParserAPI.Service.User;
using Microsoft.IdentityModel.Tokens;

namespace BookParserAPI.Security;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;

    public AuthenticationService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<string?> GetTokenAsync(LoginModel model)
    {
        var user = await _userService.GetByLoginAsync(model.Login);
        
        if (user is null)
            return null;
        if (model.Password != user.Password)
            return null;          
                
        var token = CreateToken(user);
        return token;

    }
    
    private string CreateToken(User userData)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userData.Login)
        };

        var jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

}