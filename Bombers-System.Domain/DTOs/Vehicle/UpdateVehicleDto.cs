using System.ComponentModel.DataAnnotations;

namespace Bombers_System.Domain.DTOs.Vehicle;

public class UpdateVehicleDto
{
    [Required(ErrorMessage = "La estación es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de estación debe ser mayor a 0")]
    public int StationId { get; set; }

    [Required(ErrorMessage = "El nivel de agua es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El nivel de agua no puede ser negativo")]
    public int WaterLevelGallons { get; set; }

    [Required(ErrorMessage = "La fecha de mantenimiento es obligatoria")]
    public DateOnly LastMaintenanceDate { get; set; }

    [Required(ErrorMessage = "El código de radio es obligatorio")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "El código de radio debe tener entre 1 y 50 caracteres")]
    public string RadioCode { get; set; } = null!;

    [Required(ErrorMessage = "El tipo de vehículo es obligatorio")]
    [RegularExpression("^(Bomba|Escalera|Rescate)$", ErrorMessage = "El tipo debe ser: Bomba, Escalera o Rescate")]
    public string VehicleType { get; set; } = null!;

    [Required(ErrorMessage = "El estado operativo es obligatorio")]
    [RegularExpression("^(Activo|Inactivo|Mantenimiento)$", ErrorMessage = "El estado debe ser: Activo, Inactivo o Mantenimiento")]
    public string OperationalStatus { get; set; } = null!;
}
