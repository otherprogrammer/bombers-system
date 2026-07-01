using Bombers_System.Application.UseCases.IncidentUseCases.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteIncidentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _context.CadIncidents
            .FirstOrDefaultAsync(x => x.IncidentId == request.IncidentId);

        if (incident == null)
            return false;

        _context.CadIncidents.Remove(incident);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}