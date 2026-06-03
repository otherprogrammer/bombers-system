using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Infrastructure.Adapters;

public class StationRepository : GenericRepository<Station>, IStationRepository
{
    public StationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
