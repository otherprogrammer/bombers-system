namespace Bombers_System.Domain.DTOs.Firefighter;

public class UpdateFirefighterDto
{
    public int StationId { get; set; }
    public string FullName { get; set; } = null!;
    public string MedicalCertification { get; set; } = null!;
    public string Rank { get; set; } = null!;
    public string CurrentStatus { get; set; } = null!;
}
