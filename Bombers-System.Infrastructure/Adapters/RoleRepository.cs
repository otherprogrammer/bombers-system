using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Infrastructure.Adapters;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}