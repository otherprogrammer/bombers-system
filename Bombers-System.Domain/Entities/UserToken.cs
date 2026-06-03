namespace Bombers_System.Domain.Entities;

public partial class UserToken
{
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public string TokenValue { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsRevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
