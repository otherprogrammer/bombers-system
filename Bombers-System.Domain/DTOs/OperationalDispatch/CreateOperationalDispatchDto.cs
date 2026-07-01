namespace Bombers_System.Domain.DTOs.OperationalDispatch;

public class CreateOperationalDispatchDto
{
    public int IncidentId { get; set; }
    public int VehicleId { get; set; }
    public DateTime StationAlertTime { get; set; }
    public DateTime VehicleDepartureTime { get; set; }
    public DateTime SceneArrivalTime { get; set; }
}