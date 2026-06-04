using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Infrastructure.Adapters;

public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
{
    private readonly ApplicationDbContext _context;

    public UserTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}