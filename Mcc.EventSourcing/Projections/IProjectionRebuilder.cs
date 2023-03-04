namespace Mcc.EventSourcing.Projections;

public interface IProjectionRebuilder<TEntity>
{
    Task RebuildAsync(Action<EventWrapper> action, CancellationToken cancellationToken);
}