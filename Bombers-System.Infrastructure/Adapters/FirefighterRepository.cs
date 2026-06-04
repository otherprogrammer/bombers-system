using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class FirefighterRepository : GenericRepository<Firefighter>, IFirefighterRepository
{
    private readonly ApplicationDbContext _context;

    public FirefighterRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Firefighter>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default)
    {
        return await _context.Firefighters
            .Where(f => f.StationId == stationId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Firefighters
            .AnyAsync(fp => fp.FirefighterId == id, cancellationToken);
    }
}