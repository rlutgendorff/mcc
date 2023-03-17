using Mcc.EventSourcing.Cqrs.Processors;
using Mcc.EventSourcing.Snapshots;
using Mcc.EventSourcing.Stores;

namespace Mcc.EventSourcing.Aggregates.Services;

public class AggregateService<TEntity>
    where TEntity : BaseEventSourceAggregate, new()
{
    private readonly ISnapshotService<TEntity> _snapshotService;
    private readonly IEventStore _eventStore;
    private readonly IEventSourcingProcessor _processor;

    public AggregateService(ISnapshotService<TEntity> snapshotService, IEventStore eventStore, IEventSourcingProcessor processor)
    {
        _snapshotService = snapshotService;
        _eventStore = eventStore;
        _processor = processor;
    }

    public TEntity Create()
    {
        var entity = new TEntity
        {
            Id = Guid.NewGuid()
        };

        BaseEventSourceAggregate.AddChangeTracker(entity, new BaseEventSourceAggregate.InternalChangeTracker(_processor, entity));
        return entity;
    }

    public async Task<TEntity> Load(Guid aggregateId, CancellationToken token)
    {
        var snapshot = await _snapshotService.Restore(aggregateId, token);

        if (snapshot != null)
        {
            BaseEventSourceAggregate.AddChangeTracker(snapshot, new BaseEventSourceAggregate.InternalChangeTracker(_processor, snapshot));
            return snapshot;
        }
        var entity = new TEntity
        {
            Id = aggregateId
        };

        var events = await _eventStore.ReadEventsAsync(aggregateId, token);

        BaseEventSourceAggregate.AddChangeTracker(entity, new BaseEventSourceAggregate.InternalChangeTracker(_processor, entity));

        foreach (var @event in events)
        {
            entity.ChangeTracker.Apply(@event, false);
        }

        return entity;
    }
}