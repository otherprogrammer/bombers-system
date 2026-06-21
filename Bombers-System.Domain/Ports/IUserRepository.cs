using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByFirefighterIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetWithFirefighterAsync(CancellationToken cancellationToken = default);
}