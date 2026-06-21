namespace Bombers_System.Application.DTOs.Incident;

public class IncidentDto
{
    public int IncidentId { get; set; }
    public string IncidentCode { get; set; }
    public string EmergencyType { get; set; }
    public int PriorityLevel { get; set; }
    public string Status { get; set; }
}