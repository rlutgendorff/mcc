using Mcc.Cqrs.Commands;
using Mcc.Ddd;
using Mcc.EventSourcing.Cqrs.Commands;
using Mcc.EventSourcing.Cqrs.Processors;
using Mcc.EventSourcing.Cqrs.Queries;
using Mcc.EventSourcing.Extensions;
using Mcc.EventSourcing.Validations;

namespace Mcc.EventSourcing.Aggregates;

public abstract class BaseEventSourceAggregate : BaseAggregate, IEventSourceAggregate
{
    public ulong? Version { get; protected set; }

    internal InternalChangeTracker ChangeTracker { get; private set; }

    public void Delete(DeleteNotification delete)
    {
        ChangeTracker.AddUncommittedEvent(delete, new EventMetadata { Id = Id });
    }

    //TODO make protected
    public void AddEvent(ICommand command)
    {
        var metadata = new EventMetadata { Id = Id };

        var wrapper = ChangeTracker.AddUncommittedEvent(command, metadata);
        ChangeTracker.Apply(wrapper);
    }

    public static TEntity Create<TEntity>(IEventSourcingProcessor processor)
        where TEntity : BaseEventSourceAggregate, new()
    {
        var entity = new TEntity
        {
            Id = Guid.NewGuid()
        };

        entity.ChangeTracker = new InternalChangeTracker(processor, entity);

        return entity;
    }

    public static async Task<TEntity> Load<TEntity>(Guid id, IEventSourcingProcessor processor, CancellationToken token)
        where TEntity : BaseEventSourceAggregate, new()
    {
        var entity = new TEntity
        {
            Id = id
        };

        var events = await processor.Execute(new LoadAggregateEventsQuery { AggregateId = entity.GetId() }, token);

        entity.ChangeTracker = new InternalChangeTracker(processor, entity);

        foreach (var @event in events)
        {
            entity.ChangeTracker.Apply(@event, false);
        }

        return entity;
    }

    internal class InternalChangeTracker
    {
        private readonly IList<EventWrapper> _events;
        private readonly IEventSourcingProcessor _processor;
        private readonly BaseEventSourceAggregate _entity;

        public InternalChangeTracker(IEventSourcingProcessor processor, BaseEventSourceAggregate entity)
        {
            _entity = entity;
            _processor = processor;
            _events = new List<EventWrapper>();
        }

        public void Apply(EventWrapper @event, bool shouldValidate = true)
        {
            ValidationStates validation = _processor.ExecuteEvent((dynamic)_entity, (dynamic)@event.Event, shouldValidate);

            if (validation.IsValid)
            {
                _entity.Version = @event.AggregateVersion;
            }
            else
            {
                throw new ValidationStatesException(validation);
            }
        }

        public IEnumerable<EventWrapper> GetUncommittedEvents()
        {
            return _events;
        }

        public void ClearUncommittedEvents()
        {
            _events.Clear();
        }

        public EventWrapper AddUncommittedEvent(ICommand command, EventMetadata metadata)
        {
            var wrapper = new EventWrapper
            {
                AggregateId = new AggregateId(_entity.GetType(), _entity.Id),
                AggregateVersion = ++_entity.Version,
                Event = command,
                Metadata = metadata
            };

            _entity.Version ??= 0;

            _events.Add(wrapper);
            return wrapper;
        }
    }
}