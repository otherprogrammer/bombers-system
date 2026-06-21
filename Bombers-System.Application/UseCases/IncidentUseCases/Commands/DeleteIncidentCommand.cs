using MediatR;
using Bombers_System.Domain.Entities;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Queries;

public partial class GetAllIncidentsQuery : IRequest<List<CadIncident>>
{
}