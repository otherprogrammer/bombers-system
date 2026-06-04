using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IUserTokenRepository : IGenericRepository<UserToken>
{
    Task<UserToken?> GetByValueAsync(string token, CancellationToken cancellationToken = default);
}