using System.ComponentModel.DataAnnotations;

namespace Bombers_System.Domain.DTOs.Station;

public class CreateStationDto
{
    [Required(ErrorMessage = "El número de estación es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El número de estación debe ser mayor a 0")]
    public int StationNumber { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 255 caracteres")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "La capacidad de vehículos es obligatoria")]
    [Range(1, 100, ErrorMessage = "La capacidad debe estar entre 1 y 100")]
    public int VehicleCapacity { get; set; }
}
