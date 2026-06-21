using Bombers_System.Application.DTOs.Incident;
using Bombers_System.Application.UseCases.IncidentUseCases.Queries;
using Bombers_System.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDto>
{
    private readonly ApplicationDbContext _context;

    public GetIncidentByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IncidentDto?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CadIncidents
            .Where(i => i.IncidentId == request.IncidentId)
            .Select(i => new IncidentDto
            {
                IncidentId = i.IncidentId,
                IncidentCode = i.IncidentCode,
                EmergencyType = i.EmergencyType,
                PriorityLevel = i.PriorityLevel,
                Status = i.Status
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}