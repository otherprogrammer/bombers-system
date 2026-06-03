using Bombers_System.Domain.Ports;
using Bombers_System.Infrastructure.Persistence;

namespace Bombers_System.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IStationRepository Stations { get; }
    public IVehicleRepository Vehicles { get; }
    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public IFirefighterRepository Firefighters { get; }
    
    public UnitOfWork(ApplicationDbContext context,
        IStationRepository stationRepository,
        IVehicleRepository vehicleRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository, 
        IFirefighterRepository firefighterRepository)
    {
        _context = context;
        Stations = stationRepository;
        Vehicles = vehicleRepository;
        Users = userRepository;
        Roles = roleRepository;
        Firefighters = firefighterRepository;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}