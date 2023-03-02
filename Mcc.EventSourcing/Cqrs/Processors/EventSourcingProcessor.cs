using Mcc.Cqrs;
using Mcc.Cqrs.Commands;
using Mcc.Ddd;
using Mcc.Di;
using Mcc.EventSourcing.Validations;

namespace Mcc.EventSourcing.Cqrs.Processors;

public class EventSourcingProcessor : Processor, IEventSourcingProcessor
{
    public EventSourcingProcessor(IDependencyContainer container) : base(container)
    {
    }

    public Task Notify(ICommand command, CancellationToken cancellationToken, EventMetadata metadata)
    {
        var type = typeof(INotificationHandler<>).MakeGenericType(command.GetType());

        var notifications = new List<Task>();

        List<dynamic> handlers = Container.GetInstances(type).ToList();

        handlers.ForEach(h => notifications.Add(h.HandleAsync((dynamic)command, cancellationToken, metadata)));

        return Task.WhenAll(notifications);
    }

    public ValidationStates ExecuteEvent<TEntity, TCommand>(TEntity entity, TCommand @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TCommand : class, ICommand
    {
        var validations = new ValidationStates();

        if (shouldValidate)
        {
            var validators = Container.GetInstances<IPreExecuteValidator<TEntity, TCommand>>();

            foreach (var preExecuteValidator in validators)
            {
                if (preExecuteValidator.ShouldValidate(entity, @event))
                {
                    validations.Add(preExecuteValidator.Validate(entity, @event));
                }
            }
        }

        if (validations.IsValid)
        {
            var handler = Container.GetInstance<IEventHandler<TEntity, TCommand>>();

            handler.Handle(entity, @event);
        }

        return validations;
    }
}