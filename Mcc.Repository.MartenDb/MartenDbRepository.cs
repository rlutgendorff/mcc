using Marten;
using Mcc.Ddd;
using Mcc.Repository.Databases;

namespace Mcc.Repository.MartenDb;

public class MartenDbRepository<TEntity> : IDatabaseRepository<TEntity>
    where TEntity : class, IAggregate
{
    private readonly IDocumentSession _session;

    public MartenDbRepository(IDocumentStore store)
    {
        _session = store.LightweightSession();
    }

    public Task<TEntity> GetByIdAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return _session.LoadAsync<TEntity>(entity.Id, cancellationToken);
    }

    public Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(()=> _session.Store(entity), cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => _session.Delete(entity), cancellationToken);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _session.Query<TEntity>();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.Run(() => _session.DeleteObjects(entities), cancellationToken);
    }

    public IDatabaseContextScope BeginScope()
    {
        return new MartenDbContextScope(_session);
    }
}