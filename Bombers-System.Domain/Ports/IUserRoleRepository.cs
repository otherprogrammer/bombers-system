using Bombers_System.Domain.Entities;

namespace Bombers_System.Domain.Ports;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}