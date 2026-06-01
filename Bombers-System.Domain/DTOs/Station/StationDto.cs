namespace Bombers_System.Domain.DTOs.Station;

public class StationDto
{
    public int StationId { get; set; }
    public int StationNumber { get; set; }
    public string Address { get; set; } = null!;
    public int VehicleCapacity { get; set; }
}
