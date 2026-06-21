using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Commands;

public class CreateIncidentCommand : IRequest<int>
{
    public string EmergencyType { get; set; }
    public string Description { get; set; }
    public int PriorityLevel { get; set; }
    public int DispatchId { get; set; }
}