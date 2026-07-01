using Bombers_System.Application.DTOs.Incident;
using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Queries;

public class GetAllIncidentsQuery : IRequest<List<IncidentDto>>
{
}