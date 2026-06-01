namespace Bombers_System.Domain.Ports;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}