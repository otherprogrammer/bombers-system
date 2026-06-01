using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Interfaces;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Repositories;

public class StationRepository : IStationRepository
{
    private readonly ApplicationDbContext _context;

    public StationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Station>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stations.ToListAsync(cancellationToken);
    }

    public async Task<Station?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Stations.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(Station station, CancellationToken cancellationToken = default)
    {
        await _context.Stations.AddAsync(station, cancellationToken);
    }

    public void Update(Station station)
    {
        _context.Stations.Update(station);
    }

    public void Delete(Station station)
    {
        _context.Stations.Remove(station);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
