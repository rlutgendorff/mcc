using Mcc.Cqrs;
using Mcc.Cqrs.Commands;
using Mcc.Ddd;
using Mcc.EventSourcing.Validations;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Cqrs.Processors;

public interface IEventSourcingProcessor : IProcessor
{
    Task Notify(ICommand command, CancellationToken cancellationToken, EventMetadata metadata);

    ValidationStates ExecuteEvent<TEntity, TCommand>(TEntity entity, TCommand @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TCommand : class, ICommand;
}