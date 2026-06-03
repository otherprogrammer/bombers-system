using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IFirefighterRepository : IGenericRepository<FirefighterPersonnel>
{
    Task<IEnumerable<FirefighterPersonnel>> GetByStationIdAsync(int stationId, CancellationToken cancellationToken = default);
}