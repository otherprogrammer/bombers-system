using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Interfaces;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.ToListAsync(cancellationToken);
    }

    public async Task<Vehicle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default)
    {
        return await _context.Vehicles
            .Where(v => v.StationId == stationId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        await _context.Vehicles.AddAsync(vehicle, cancellationToken);
    }

    public void Update(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
    }

    public void Delete(Vehicle vehicle)
    {
        _context.Vehicles.Remove(vehicle);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
