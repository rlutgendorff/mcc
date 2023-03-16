namespace Mcc.EventSourcing.Projections;

public interface IProjectionRebuilder<TEntity>
{
    Task RebuildAsync(Func<EventWrapper, Task> action, CancellationToken cancellationToken);
}