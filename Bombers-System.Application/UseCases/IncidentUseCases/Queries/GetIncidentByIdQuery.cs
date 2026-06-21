using MediatR;
using Bombers_System.Domain.Entities;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Queries;

public class GetIncidentByIdQuery : IRequest<CadIncident>
{
    public int IncidentId { get; set; }
}