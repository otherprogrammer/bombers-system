using System;
using System.Collections.Generic;

namespace Bombers_System.Infrastructure.Persistence.Models;

public partial class PpeEquipment
{
    public int EquipmentId { get; set; }

    public int FirefighterId { get; set; }

    public DateOnly ManufacturingDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public bool DecontaminationStatus { get; set; }

    public string Type { get; set; } = null!;

    public virtual FirefighterPersonnel Firefighter { get; set; } = null!;
}
