using MediatR;
using Bombers_System.Application.DTOs.Incident;

public class GetIncidentByIdQuery : IRequest<IncidentDto>
{
    public int IncidentId { get; set; }
}