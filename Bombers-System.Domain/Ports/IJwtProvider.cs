namespace Bombers_System.Domain.Ports;

public interface IJwtProvider
{
    string GenerateToken(string userId, string username, IEnumerable<string> roles);
    string GenerateRefreshToken();
}