using NetTopologySuite.Geometries;

namespace Bombers_System.Domain.Entities;

public partial class Station
{
    public int StationId { get; set; }

    public int StationNumber { get; set; }

    public string Address { get; set; } = null!;

    public Geometry? CoverageZonePolygon { get; set; }

    public int VehicleCapacity { get; set; }

    public virtual ICollection<FirefighterPersonnel> FirefighterPersonnel { get; set; } = new List<FirefighterPersonnel>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
