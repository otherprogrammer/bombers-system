using System;
using System.Collections.Generic;

namespace Bombers_System.Infrastructure.Persistence.Models;

public partial class OperationalDispatch
{
    public int DispatchId { get; set; }

    public int IncidentId { get; set; }

    public int VehicleId { get; set; }

    public DateTime StationAlertTime { get; set; }

    public DateTime VehicleDepartureTime { get; set; }

    public DateTime SceneArrivalTime { get; set; }

    public virtual ICollection<CadIncident> CadIncidents { get; set; } = new List<CadIncident>();

    public virtual ICollection<DispatchCrew> DispatchCrews { get; set; } = new List<DispatchCrew>();

    public virtual CadIncident Incident { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
