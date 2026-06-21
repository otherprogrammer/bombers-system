using MediatR;
using NetTopologySuite.Geometries;
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
        if (request.Dto == null)
            throw new ArgumentNullException(nameof(request.Dto));

        // 📍 Crear punto GPS (lon, lat)
        var point = new Point(request.Dto.Longitude, request.Dto.Latitude)
        {
            SRID = 4326
        };

        var incident = new CadIncident
        {
            IncidentCode = "INC-" + Guid.NewGuid().ToString("N")[..6],
            EmergencyType = request.Dto.EmergencyType,
            PriorityLevel = request.Dto.PriorityLevel,
            Call911Time = DateTime.UtcNow,
            Status = "Reportado",
            GpsCoordinates = point
        };

        _context.CadIncidents.Add(incident);
        await _context.SaveChangesAsync(cancellationToken);

        return incident.IncidentId;
    }
}