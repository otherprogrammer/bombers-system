namespace Bombers_System.Domain.DTOs.Vehicle;

public class VehicleDto
{
    public int VehicleId { get; set; }
    public int StationId { get; set; }
    public int WaterLevelGallons { get; set; }
    public DateOnly LastMaintenanceDate { get; set; }
    public string RadioCode { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string OperationalStatus { get; set; } = null!;
}
