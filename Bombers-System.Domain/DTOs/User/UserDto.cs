namespace Bombers_System.Domain.DTOs.User;

public class UserDto
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;
    
    public int? FirefighterId { get; set; }
}