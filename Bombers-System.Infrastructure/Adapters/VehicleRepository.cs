using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .Where(v => v.StationId == stationId)
            .ToListAsync(cancellationToken);
    }
}
