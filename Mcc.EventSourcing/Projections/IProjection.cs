namespace Mcc.EventSourcing.Projections;

public interface IProjection<out TEntity>
{
    IQueryable<TEntity> Get();

    Task RebuildAsync(CancellationToken cancellationToken);
}