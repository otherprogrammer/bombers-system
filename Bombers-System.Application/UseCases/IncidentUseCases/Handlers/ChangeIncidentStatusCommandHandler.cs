using Bombers_System.Application.UseCases.IncidentUseCases.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class ChangeIncidentStatusCommandHandler : IRequestHandler<ChangeIncidentStatusCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public ChangeIncidentStatusCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ChangeIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        var incident = await _context.CadIncidents
            .FirstOrDefaultAsync(x => x.IncidentId == request.IncidentId);

        if (incident == null)
            return false;

        incident.IncidentCode = request.Status; // o mejor: crear campo Status si existe

        if (request.Status == "Cerrado")
        {
            incident.IncidentClosureTime = DateTime.Now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}