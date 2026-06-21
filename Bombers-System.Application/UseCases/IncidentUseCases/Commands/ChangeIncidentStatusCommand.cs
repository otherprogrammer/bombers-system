using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Commands;

public class ChangeIncidentStatusCommand : IRequest<bool>
{
    public int IncidentId { get; set; }
    public string Status { get; set; }
}