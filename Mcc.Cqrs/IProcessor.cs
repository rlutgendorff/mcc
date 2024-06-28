using Mcc.Cqrs.Commands;
using Mcc.Cqrs.Events;
using Mcc.Cqrs.Events.Validations;
using Mcc.Cqrs.Queries;
using Mcc.Ddd;
using Mcc.Di;

namespace Mcc.Cqrs;

public interface IProcessor
{
    public IDependencyContainer Container { get; }

    Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

    Task<ICommandResponse<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);

    Task Notify(IEvent command, EventMetadata metadata, CancellationToken cancellationToken);

    ValidationStates ExecuteEvent<TEntity, TEvent>(TEntity entity, TEvent @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TEvent : class, IEvent;
}