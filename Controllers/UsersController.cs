using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Models;

namespace TMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "MANAGER")]
public class UsersController : ControllerBase
{
    private readonly TMSContext _context;

    public UsersController(TMSContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<User> GetUsers()
    {
        var users = _context.Users.ToList();
        return users;
    }

    [HttpPost]
    public User PostUser(UserCreateDTO userDto)
    {
        User u = new User();
        u.Name = userDto.Name;
        u.Email = userDto.Email;

        u.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        u.RoleId = userDto.RoleId;

        _context.Users.Add(u);
        _context.SaveChanges();

        return u;
    }
}

