namespace Mcc.EventSourcing.Views;

public interface IView<out TEntity>
{
    IQueryable<TEntity> Get();

    Task RebuildAsync(CancellationToken cancellationToken);
}