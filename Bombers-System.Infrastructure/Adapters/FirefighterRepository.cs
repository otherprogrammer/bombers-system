using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class FirefighterRepository : GenericRepository<FirefighterPersonnel>, IFirefighterRepository
{
    private readonly ApplicationDbContext _context;

    public FirefighterRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FirefighterPersonnel>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default)
    {
        return await _context.FirefighterPersonnel
            .Where(f => f.StationId == stationId)
            .ToListAsync(cancellationToken);
    }
}