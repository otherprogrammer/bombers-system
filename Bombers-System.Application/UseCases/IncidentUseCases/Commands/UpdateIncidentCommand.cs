using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Commands;

public class UpdateIncidentCommand : IRequest<bool>
{
    public int IncidentId { get; set; }
    public string EmergencyType { get; set; }
    public string Description { get; set; }
    public int PriorityLevel { get; set; }
}