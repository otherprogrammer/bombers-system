namespace Bombers_System.Domain.Ports;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken  = default);
    Task AddAsync(T entity, CancellationToken cancellationToken  = default);
    void Update(T entity);
    void Delete(T entity);
}