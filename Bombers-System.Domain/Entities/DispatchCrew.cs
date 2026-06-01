namespace Bombers_System.Domain.Entities;

public partial class DispatchCrew
{
    public int CrewId { get; set; }

    public int DispatchId { get; set; }

    public int FirefighterId { get; set; }

    public string VehiclePosition { get; set; } = null!;

    public virtual OperationalDispatch Dispatch { get; set; } = null!;

    public virtual FirefighterPersonnel Firefighter { get; set; } = null!;
}
