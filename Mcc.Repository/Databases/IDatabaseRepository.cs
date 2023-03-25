namespace Mcc.Repository.Databases;

public interface IDatabaseRepository<TEntity> : IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(TEntity entity, CancellationToken cancellationToken);

    IQueryable<TEntity> GetAll();

    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    IDatabaseContextScope BeginScope();
}