using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookParserAPI.Config;

public class AuthOptions
{
    private const string Key = "mySecretKey123asdsadasdasdasdasdasdad"; // todo keep in secret

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
        => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}