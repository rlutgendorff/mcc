using Mcc.Cqrs;
using Mcc.Cqrs.Events;
using Mcc.Cqrs.Events.Validations;
using Mcc.Ddd;
using Mcc.EventSourcing.Cqrs.Commands;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Aggregates;

public abstract class BaseEventSourceAggregate : BaseAggregate, IEventSourceAggregate
{
    public long? Version { get; set; }

    internal InternalChangeTracker ChangeTracker { get; private set; }

    public void Delete(DeleteNotification delete)
    {
        ChangeTracker.AddUncommittedEvent(delete, new EventMetadata { Id = Id, TypeName = delete.GetType().AssemblyQualifiedName});
    }

    protected void AddEvent(IEvent @event)
    {
        var metadata = new EventMetadata { Id = Id, TypeName = @event.GetType().AssemblyQualifiedName};

        var wrapper = ChangeTracker.AddUncommittedEvent(@event, metadata);
        ChangeTracker.Apply(wrapper);
    }

    internal static void AddChangeTracker<TEntity>(TEntity entity, InternalChangeTracker tracker)
        where TEntity : BaseEventSourceAggregate
    {
        entity.ChangeTracker = tracker;
    }

    internal class InternalChangeTracker
    {
        private readonly IList<EventWrapper> _events;
        private readonly IProcessor _processor;
        private readonly BaseEventSourceAggregate _entity;

        public InternalChangeTracker(IProcessor processor, BaseEventSourceAggregate entity)
        {
            _entity = entity;
            _processor = processor;
            _events = new List<EventWrapper>();
        }

        public void Apply(EventWrapper @event, bool shouldValidate = true)
        {
            ValidationStates validation = _processor.ExecuteEvent((dynamic)_entity, @event.Event, shouldValidate);

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

        public EventWrapper AddUncommittedEvent(IEvent command, EventMetadata metadata)
        {
            var wrapper = new EventWrapper
            {
                AggregateId =  _entity.Id,
                AggregateVersion = ++_entity.Version ?? 0,
                Event = command,
                Metadata = metadata
            };

            _events.Add(wrapper);
            return wrapper;
        }
    }
}