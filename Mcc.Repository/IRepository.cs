namespace Mcc.Repository;

public interface IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(Guid id);
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}