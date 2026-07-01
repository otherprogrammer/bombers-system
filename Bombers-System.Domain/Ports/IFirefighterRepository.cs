using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IFirefighterRepository : IGenericRepository<Firefighter>
{
    Task<IEnumerable<Firefighter>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
}