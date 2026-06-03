namespace Bombers_System.Domain.DTOs.Firefighter;

public class CreateFirefighterDto
{
    public int FirefighterId { get; set; }
    public int StationId { get; set; }
    public string FullName { get; set; } = null!;
    public string MedicalCertification { get; set; } = null!; // Equivale a Especialidad/Certificación
    public DateOnly HireDate { get; set; }
    public string Rank { get; set; } = null!; // Rango (Capitán, Teniente, Bombero)
    public string CurrentStatus { get; set; } = null!; // Estado (Activo, Descanso, Baja)
}
