namespace Bombers_System.Domain.Entities;

public partial class Firefighter
{
    public int FirefighterId { get; set; }

    public int StationId { get; set; }

    public string FullName { get; set; } = null!;

    public string MedicalCertification { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public string Rank { get; set; } = null!;

    public string CurrentStatus { get; set; } = null!;

    public virtual ICollection<DispatchCrew> DispatchCrews { get; set; } = new List<DispatchCrew>();

    public virtual ICollection<DutyShift> DutyShifts { get; set; } = new List<DutyShift>();

    public virtual ICollection<PostIncidentReport> PostIncidentReports { get; set; } = new List<PostIncidentReport>();

    public virtual ICollection<PpeEquipment> PpeEquipments { get; set; } = new List<PpeEquipment>();

    public virtual Station Station { get; set; } = null!;

    public virtual User? User { get; set; }
}
