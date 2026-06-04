using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> ExistsByFirefighterIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.FirefighterId == id, cancellationToken);
    }

    public Task AssignRoleAsync(User user, int roleId)
    {
        user.UserRoles.Add(new UserRole
        {
            RoleId = roleId,
            AssignedAt =  DateTime.UtcNow
        });
        return Task.CompletedTask;
    }
}