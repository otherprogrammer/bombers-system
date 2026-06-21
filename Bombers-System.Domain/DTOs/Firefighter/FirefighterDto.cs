namespace Bombers_System.Domain.DTOs.Firefighter;

public class FirefighterDto
{
    public int FirefighterId { get; set; }
    public int StationId { get; set; }
    public string FullName { get; set; } = null!;
    public string MedicalCertification { get; set; } = null!;
    public DateOnly HireDate { get; set; }
    public string Rank { get; set; } = null!;
    public string CurrentStatus { get; set; } = null!;
}