namespace Mcc.EventSourcing.Views;

public interface IViewRebuilder<TEntity>
{
    Task RebuildAsync(Action<EventWrapper> action, CancellationToken cancellationToken);
}