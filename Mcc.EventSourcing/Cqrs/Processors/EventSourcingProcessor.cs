using Mcc.Cqrs;
using Mcc.Ddd;
using Mcc.Di;
using Mcc.EventSourcing.Exceptions;
using Mcc.EventSourcing.Validations;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Cqrs.Processors;

public class EventSourcingProcessor : Processor, IEventSourcingProcessor
{
    public EventSourcingProcessor(IDependencyContainer container) : base(container)
    {
    }

    public Task Notify(IEvent command, CancellationToken cancellationToken, EventMetadata metadata)
    {
        var type = typeof(INotificationHandler<>).MakeGenericType(command.GetType());

        var tasks = new List<Task>();

        var handlers = Container.GetInstances(type).ToList();

        if (handlers.Count == 0)
        {
            throw new NoNotificationHandlerException($"No notificationHandler For type '{type.AssemblyQualifiedName}'");
        }

        foreach (var handler in handlers)
        {
            var methods = handler.GetType().GetMethods().Where(x => x.Name == "HandleAsync").ToList();

            var method = methods.First();

            if (methods.Count() > 1)
            {
                foreach (var methodInfo in methods)
                {
                    var methodParameters = methodInfo.GetParameters();

                    if (methodParameters[0].ParameterType == command.GetType())
                    {
                        method = methodInfo;
                        break;
                    }
                }
            }

            var parameters = new object[]
            {
                command,
                cancellationToken,
                metadata
            };

            var i = method.Invoke(handler, parameters);

            if (i is Task task)
            {
                tasks.Add(task);
            }
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public ValidationStates ExecuteEvent<TEntity, TEvent>(TEntity entity, TEvent @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TEvent : class, IEvent
    {
        var validations = new ValidationStates();

        if (shouldValidate)
        {
            var validators = Container.GetInstances<IPreExecuteValidator<TEntity, TEvent>>();

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
            var handler = Container.GetInstance<IEventHandler<TEntity, TEvent>>();

            handler.Handle(entity, @event);
        }

        return validations;
    }
}