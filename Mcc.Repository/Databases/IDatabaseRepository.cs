namespace Mcc.Repository.Databases;

public interface IDatabaseRepository<TEntity> : IRepository<TEntity>
{
    IQueryable<TEntity> GetAll();

    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    IDatabaseContextScope BeginScope();
}