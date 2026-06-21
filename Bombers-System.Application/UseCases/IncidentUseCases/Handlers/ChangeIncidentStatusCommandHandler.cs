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
        // 🔴 Validar DTO
        if (request.Dto == null)
            return false;

        // 🟡 Validar estados permitidos
        var validStatuses = new[] { "Reportado", "EnAtencion", "Cerrado" };

        if (!validStatuses.Contains(request.Dto.Status))
            return false;

        // 🔵 Buscar incidente
        var incident = await _context.CadIncidents
            .FirstOrDefaultAsync(x => x.IncidentId == request.Dto.IncidentId, cancellationToken);

        if (incident == null)
            return false;

        // 🟢 Cambiar estado
        incident.Status = request.Dto.Status;

        // 🔥 Si se cierra el incidente, registrar hora de cierre
        if (request.Dto.Status == "Cerrado")
        {
            incident.IncidentClosureTime = DateTime.Now;
        }

        // 💾 Guardar cambios
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}