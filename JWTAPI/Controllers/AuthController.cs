using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWTAPI.Context;
using JWTAPI.Entities;
using JWTAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWTAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
 
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
 
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        var created = await _authService.RegisterAsync(user);
        if (created == null)
            return BadRequest("Пользователь с таким логином уже существует");
 
        return Ok(new
        {
            created.Id,
            created.Login,
            created.Role
        });
    }
 
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthData data)
    {
        var result = await _authService.LoginAsync(data);
        if (result == null)
            return Unauthorized();
 
        return Ok(new
        {
            User = result.User,
            Token = result.Token
        });
    }
}