using Mcc.Cqrs;
using Mcc.Ddd;
using Mcc.EventSourcing.Validations;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Cqrs.Processors;

public interface IEventSourcingProcessor : IProcessor
{
    Task Notify(IEvent command, CancellationToken cancellationToken, EventMetadata metadata);

    ValidationStates ExecuteEvent<TEntity, TEvent>(TEntity entity, TEvent @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TEvent : class, IEvent;
}