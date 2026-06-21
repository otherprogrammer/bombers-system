using MediatR;
using Bombers_System.Domain.Entities;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Queries;

public class GetAllIncidentsQuery : IRequest<List<CadIncident>>
{
}