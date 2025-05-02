using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenkey = config["TokenKey"] ?? throw new Exception("Cannot access tokenkey from appsetting");
        if (tokenkey.Length < 64) throw new Exception("Your tokenkey needs to be longger");
        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.UserName)
        };

        var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandlar = new JwtSecurityTokenHandler();
        var token = tokenHandlar.CreateToken(tokenDescriptor);

        return tokenHandlar.WriteToken(token);
 }
}
