namespace Mcc.Repository;

public interface IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(TEntity entity, CancellationToken cancellationToken);
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}