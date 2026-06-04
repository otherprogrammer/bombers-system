namespace Bombers_System.Domain.Ports;

public interface IUnitOfWork
{
    IStationRepository Stations { get; }
    IVehicleRepository Vehicles { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IFirefighterRepository Firefighters { get; }
    IUserTokenRepository UserTokens { get; }
    IUserRoleRepository UserRoles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}