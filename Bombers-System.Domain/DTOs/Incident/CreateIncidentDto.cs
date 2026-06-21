namespace Bombers_System.Application.DTOs.Incident;

public class CreateIncidentDto
{
    public string EmergencyType { get; set; }
    public int PriorityLevel { get; set; }
    public double  Latitude { get; set; }
    public double  Longitude { get; set; }
}