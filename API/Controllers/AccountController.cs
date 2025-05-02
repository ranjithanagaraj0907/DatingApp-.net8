
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class AccountController : BaseApiController
{
    private readonly DataContext context;
    private readonly ITokenService tokenService;
    public AccountController(DataContext _context, ITokenService _tokenService)
    {
        context = _context;
        tokenService = _tokenService;
    }

    [HttpPost("register")]

    public async Task<ActionResult<UserDto>> Register(RegisterDto register)
    {

        if (await UserExist(register.UserName))
        {
            return BadRequest("Username is taken !");
        }
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = register.UserName,
            Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
            PasswordHash = hmac.Key

        };

        context.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    [HttpPost("Login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto login)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == login.Username.ToLower());
        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.Password);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != user.Password[i])
            {
                return Unauthorized("Invalid Password");

            }
        }
        return new UserDto
        {
            username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExist(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

}
