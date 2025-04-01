﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]  // /api/users
public class UsersController :ControllerBase
{
    private readonly DataContext _context;
    public  UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id:int}")]  // Get user by Id
    public async Task<ActionResult<AppUser>> GetUserById(int id)
    {
        var users =await _context.Users.FindAsync(id);

        if (users == null) return NotFound();
        return users;
    }
}
