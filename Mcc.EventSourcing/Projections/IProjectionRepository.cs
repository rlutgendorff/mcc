namespace Mcc.EventSourcing.Projections;

public interface IProjectionRepository<out TEntity>
{
    IQueryable<TEntity> Get();

    Task RebuildAsync(CancellationToken cancellationToken);
}