using Bombers_System.Application.UseCases.IncidentUseCases.Commands;
using MediatR;
using Bombers_System.Domain.Entities;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Handlers;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, int>
{
    private readonly ApplicationDbContext _context;

    public CreateIncidentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = new CadIncident
        {
            EmergencyType = request.EmergencyType,
            PriorityLevel = request.PriorityLevel,
            DispatchId = request.DispatchId,
            Call911Time = DateTime.Now
        };

        _context.CadIncidents.Add(incident);
        await _context.SaveChangesAsync(cancellationToken);

        return incident.IncidentId;
    }
}