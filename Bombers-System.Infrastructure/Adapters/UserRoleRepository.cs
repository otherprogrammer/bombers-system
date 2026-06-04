using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.RoleName)
            .ToListAsync(cancellationToken);
    }
}