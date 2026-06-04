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
    public IUserTokenRepository UserTokens { get; }
    
    public UnitOfWork(ApplicationDbContext context,
        IStationRepository stationRepository,
        IVehicleRepository vehicleRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository, 
        IFirefighterRepository firefighterRepository,
        IUserTokenRepository userTokenRepository)
    {
        _context = context;
        Stations = stationRepository;
        Vehicles = vehicleRepository;
        Users = userRepository;
        Roles = roleRepository;
        Firefighters = firefighterRepository;
        UserTokens = userTokenRepository;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}