namespace Bombers_System.Domain.DTOs.Station;

public class UpdateStationDto
{
    public int StationNumber { get; set; }
    public string Address { get; set; } = null!;
    public int VehicleCapacity { get; set; }
}
