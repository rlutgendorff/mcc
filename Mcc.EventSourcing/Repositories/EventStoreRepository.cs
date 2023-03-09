using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Aggregates.Services;
using Mcc.EventSourcing.Cqrs.Commands;
using Mcc.EventSourcing.Cqrs.Processors;
using Mcc.EventSourcing.Extensions;
using Mcc.EventSourcing.Stores;
using Mcc.Extensions;
using Mcc.Repository;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Repositories;

public class EventStoreRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEventSourceAggregate
{
    private readonly IEventStore _eventStore;
    private readonly IEventSourcingProcessor _processor;
    private readonly IEventPublisher _publisher;
    private readonly AggregateEventsService _eventService;

    public EventStoreRepository(IEventStore eventStore, IEventSourcingProcessor processor, AggregateEventsService eventService, IEventPublisher publisher)
    {
        _eventStore = eventStore;
        _processor = processor;
        _eventService = eventService;
        _publisher = publisher;
    }

    public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var events = _eventService.GetUncommittedEvents(entity);

        foreach (var uncommittedEvent in events)
        {
            await _eventStore.AppendEventAsync(uncommittedEvent, cancellationToken);
            _publisher.Publish(entity.GetType().Name, uncommittedEvent.Event.GetType().Name,
                new Message { Data = uncommittedEvent.Event.ToJson(), Metadata = uncommittedEvent.Metadata });
        }


        _eventService.ClearUncommittedEvents(entity);
    }

    public async Task DeleteAsync(TEntity data, CancellationToken cancellationToken)
    {
        var @event = new DeleteNotification<TEntity> { Id = data.Id };

        data.Delete(@event);

        await SaveAsync(data, cancellationToken);

        await _eventStore.DeleteAsync(data.GetId(), cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var events = await _eventStore.ReadEventsAsync(entity.GetId(), cancellationToken);

        foreach (var @event in events)
        {
            _eventService.Apply(entity, @event, false);
        }

        return entity;
    }
}