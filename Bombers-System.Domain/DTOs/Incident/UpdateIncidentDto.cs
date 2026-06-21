namespace Bombers_System.Application.DTOs.Incident;

public class UpdateIncidentDto
{
    public int IncidentId { get; set; }
    public string EmergencyType { get; set; }
    public int PriorityLevel { get; set; }
}