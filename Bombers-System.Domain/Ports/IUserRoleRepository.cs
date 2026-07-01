using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserRole?> GetByIdsAsync(int userId, int roleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdsAsync(int userId, int roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
}