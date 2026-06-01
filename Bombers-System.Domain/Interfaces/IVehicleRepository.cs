using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Vehicle?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default);
    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    void Update(Vehicle vehicle);
    void Delete(Vehicle vehicle);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
