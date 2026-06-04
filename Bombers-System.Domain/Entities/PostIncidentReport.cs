namespace Bombers_System.Domain.Entities;

public partial class PostIncidentReport
{
    public int ReportId { get; set; }

    public int IncidentId { get; set; }

    public int OfficerInChargeId { get; set; }

    public string TacticalSummary { get; set; } = null!;

    public int TotalWaterUsed { get; set; }

    public bool InjuriesReported { get; set; }

    public DateTime StationReturnTime { get; set; }

    public virtual CadIncident Incident { get; set; } = null!;

    public virtual Firefighter OfficerInCharge { get; set; } = null!;
}
