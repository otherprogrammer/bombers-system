namespace Bombers_System.Domain.DTOs.User;

public class UpdateUserDto
{

    public string Username { get; set; } = null!;
    
    public int? FirefighterId { get; set; }
}