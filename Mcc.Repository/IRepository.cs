namespace Mcc.Repository;

public interface IRepository<in TEntity>
{
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}