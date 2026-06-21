using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<bool> ExistsByRoleNameAsync(string rolename, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
}