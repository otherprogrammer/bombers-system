using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Commands;

public class DeleteIncidentCommand : IRequest<bool>
{
    public int IncidentId { get; set; }
}