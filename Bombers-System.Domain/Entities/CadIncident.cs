using NetTopologySuite.Geometries;

namespace Bombers_System.Domain.Entities;

public partial class CadIncident
{
    public int IncidentId { get; set; }

    public string IncidentCode { get; set; } = null!;

    public string EmergencyType { get; set; } = null!;

    public int PriorityLevel { get; set; }

    public Geometry GpsCoordinates { get; set; } = null!;

    public DateTime Call911Time { get; set; }

    public DateTime? IncidentClosureTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OperationalDispatch> OperationalDispatches { get; set; } = new List<OperationalDispatch>();

    public virtual ICollection<PostIncidentReport> PostIncidentReports { get; set; } = new List<PostIncidentReport>();
}
