using JWTAPI.Entities;

namespace JWTAPI.Services;

public record LoginUserDto(int Id, string Login, int Role, string RoleName);
public record LoginResult(LoginUserDto User, string Token);
 
public interface IAuthService
{
    Task<User?> RegisterAsync(User user);
 
    Task<LoginResult?> LoginAsync(AuthData data);
}