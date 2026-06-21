using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsByRoleNameAsync(string rolename, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AnyAsync(r => r.RoleName == rolename, cancellationToken);
    }
    
    public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AnyAsync(r => r.RoleId == id, cancellationToken);
    }
}