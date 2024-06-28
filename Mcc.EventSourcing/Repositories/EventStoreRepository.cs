using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Aggregates.Services;
using Mcc.EventSourcing.Cqrs.Commands;
using Mcc.EventSourcing.Snapshots;
using Mcc.EventSourcing.Stores;
using Mcc.Extensions;
using Mcc.Repository;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Repositories;

public class EventStoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEventSourceAggregate
{
    private readonly IEventStore _eventStore;
    private readonly IEventPublisher _publisher;
    private readonly AggregateEventsService _eventService;
    private readonly ISnapshotService<TEntity> _snapshotService;

    public EventStoreRepository(IEventStore eventStore, AggregateEventsService eventService, IEventPublisher publisher, ISnapshotService<TEntity> snapshotService)
    {
        _eventStore = eventStore;
        _eventService = eventService;
        _publisher = publisher;
        _snapshotService = snapshotService;
    }

    public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var events = _eventService.GetUncommittedEvents(entity);

        foreach (var uncommittedEvent in events)
        {
            await _eventStore.AppendEventAsync(uncommittedEvent, cancellationToken);
            _publisher.Publish(uncommittedEvent);
        }

        _eventService.ClearUncommittedEvents(entity);
        await _snapshotService.Store(entity, cancellationToken);
    }

    public async Task DeleteAsync(TEntity data, CancellationToken cancellationToken)
    {
        var @event = new DeleteNotification<TEntity> { Id = data.Id };

        data.Delete(@event);

        await SaveAsync(data, cancellationToken);

        await _eventStore.DeleteAsync(data.Id, cancellationToken);
    }
}