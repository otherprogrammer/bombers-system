using MediatR;
using Bombers_System.Application.DTOs.Incident;

public class ChangeIncidentStatusCommand : IRequest<bool>
{
    public ChangeIncidentStatusDto Dto { get; set; }
}