using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IVehicleRepository : IGenericRepository<Vehicle>
{
    Task<IEnumerable<Vehicle>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default);
}
