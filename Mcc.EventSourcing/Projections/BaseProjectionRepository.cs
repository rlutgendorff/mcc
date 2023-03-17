using Mcc.Repository.Databases;

namespace Mcc.EventSourcing.Projections;

public abstract class BaseProjectionRepository<TEntity> : IProjectionRepository<TEntity>
{
    protected readonly IDatabaseRepository<TEntity> Repository;

    protected BaseProjectionRepository(IDatabaseRepository<TEntity> repository)
    {
        Repository = repository;
    }

    public IQueryable<TEntity> Get()
    {
        return Repository.GetAll();
    }

    public abstract Task RebuildAsync(CancellationToken cancellationToken);
}