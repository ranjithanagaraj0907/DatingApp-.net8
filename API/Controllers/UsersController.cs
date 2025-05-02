using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]  // /api/users
public class UsersController : BaseApiController
{
    private readonly DataContext context;
    public UsersController(DataContext _context)
    {
        context = _context;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }
    [Authorize]
    [HttpGet("{id:int}")]  // Get user by Id
    public async Task<ActionResult<AppUser>> GetUserById(int id)
    {
        var users = await context.Users.FindAsync(id);

        if (users == null) return NotFound();
        return users;
    }
}
