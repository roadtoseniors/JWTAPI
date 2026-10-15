using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWTAPI.Context;
using JWTAPI.Entities;
using JWTAPI.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWTAPI.Services;

public class AuthService : IAuthService
{
    private readonly MyDbContext _context;
 
    public AuthService(MyDbContext context)
    {
        _context = context;
    }
 
    public async Task<User?> RegisterAsync(User user)
    {
        var exists = await _context.Users.AnyAsync(x => x.Login == user.Login);
        if (exists)
            return null;
 
        user.Role = 1;
 
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
 
        return user;
    }
 
    public async Task<LoginResult?> LoginAsync(AuthData data)
    {
        var user = await _context.Users
            .Where(u => u.Login == data.Login && u.Password == data.Password)
            .Select(u => new LoginUserDto(u.Id, u.Login, u.Role, u.RoleNavigation.Role1))
            .FirstOrDefaultAsync();
 
        if (user == null)
            return null;
 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
 
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(365),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
 
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
 
        return new LoginResult(user, token);
    }
}