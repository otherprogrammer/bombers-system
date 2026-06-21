using Bombers_System.Application.UseCases.IncidentUseCases.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Bombers_System.Infrastructure.Persistence;
using Bombers_System.Domain.Entities;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, List<CadIncident>>
{
    private readonly ApplicationDbContext _context;

    public GetAllIncidentsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CadIncident>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.CadIncidents.ToListAsync(cancellationToken);
    }
}