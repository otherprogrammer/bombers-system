using Bombers_System.Application.UseCases.IncidentUseCases.Commands;
using Bombers_System.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public UpdateIncidentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        if (request.Dto == null)
            return false;

        var incident = await _context.CadIncidents
            .FirstOrDefaultAsync(i => i.IncidentId == request.Dto.IncidentId, cancellationToken);

        if (incident == null)
            return false;

        incident.EmergencyType = request.Dto.EmergencyType;
        incident.PriorityLevel = request.Dto.PriorityLevel;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}