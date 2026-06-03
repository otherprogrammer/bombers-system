namespace Bombers_System.Domain.Ports;

public interface IUnitOfWork
{
    IStationRepository Stations { get; }
    IVehicleRepository Vehicles { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}