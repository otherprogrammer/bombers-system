namespace Bombers_System.Domain.DTOs.OperationalDispatch;

public class UpdateOperationalDispatchDto
{
    public int VehicleId { get; set; }
    public DateTime StationAlertTime { get; set; }
    public DateTime VehicleDepartureTime { get; set; }
    public DateTime SceneArrivalTime { get; set; }
}