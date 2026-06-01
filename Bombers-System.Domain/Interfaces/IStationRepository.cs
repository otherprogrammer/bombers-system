using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Interfaces;

public interface IStationRepository
{
    Task<IEnumerable<Station>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Station?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Station station, CancellationToken cancellationToken = default);
    void Update(Station station);
    void Delete(Station station);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
