using Bombers_System.Application.DTOs.Incident;
using Bombers_System.Application.UseCases.IncidentUseCases.Queries;
using Bombers_System.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, List<IncidentDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllIncidentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IncidentDto>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.CadIncidents
            .Select(i => new IncidentDto
            {
                IncidentId = i.IncidentId,
                IncidentCode = i.IncidentCode,
                EmergencyType = i.EmergencyType,
                PriorityLevel = i.PriorityLevel,
                Status = i.Status
            })
            .ToListAsync(cancellationToken);
    }
}