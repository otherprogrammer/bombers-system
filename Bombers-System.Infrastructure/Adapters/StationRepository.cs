using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bombers_System.Infrastructure.Adapters;

public class StationRepository : GenericRepository<Station>,IStationRepository
{
    private readonly ApplicationDbContext _context;

    public StationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}