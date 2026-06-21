using MediatR;
using Bombers_System.Application.DTOs.Incident;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Queries;

public class GetIncidentByIdQuery : IRequest<IncidentDto>
{
    public int IncidentId { get; set; }
}