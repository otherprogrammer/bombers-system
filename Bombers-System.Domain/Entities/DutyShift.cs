namespace Bombers_System.Domain.Entities;

public partial class DutyShift
{
    public int ShiftId { get; set; }

    public int FirefighterId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal HoursWorked { get; set; }

    public string RolAssigned { get; set; } = null!;

    public virtual Firefighter Firefighter { get; set; } = null!;
}
