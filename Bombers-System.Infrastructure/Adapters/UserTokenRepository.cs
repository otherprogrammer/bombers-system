using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
{
    private readonly ApplicationDbContext _context;

    public UserTokenRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserToken?> GetByValueAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.UserTokens
            .AsNoTracking()
            .Where(ut => ut.TokenValue == token)
            .FirstOrDefaultAsync(cancellationToken);
    }
}