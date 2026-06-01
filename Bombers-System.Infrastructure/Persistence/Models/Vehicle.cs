using System;
using System.Collections.Generic;

namespace Bombers_System.Infrastructure.Persistence.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public int StationId { get; set; }

    public int WaterLevelGallons { get; set; }

    public DateOnly LastMaintenanceDate { get; set; }

    public string RadioCode { get; set; } = null!;

    public string VehicleType { get; set; } = null!;

    public string OperationalStatus { get; set; } = null!;

    public virtual ICollection<OperationalDispatch> OperationalDispatches { get; set; } = new List<OperationalDispatch>();

    public virtual Station Station { get; set; } = null!;
}
