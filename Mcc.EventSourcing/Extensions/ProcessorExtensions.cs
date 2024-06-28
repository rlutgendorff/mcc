using Mcc.Ddd;
using Mcc.EventSourcing.Cqrs;
using Mcc.EventSourcing.Exceptions;
using Mcc.EventSourcing.ServiceBus;
using Mcc.EventSourcing.Validations;
using Mcc.Cqrs;

namespace Mcc.EventSourcing.Extensions;

public static class ProcessorExtensions
{
    public static Task Notify(this IProcessor processor, IEvent command, CancellationToken cancellationToken, EventMetadata metadata)
    {
        var type = typeof(INotificationHandler<>).MakeGenericType(command.GetType());

        var tasks = new List<Task>();

        var handlers = processor.Container.GetInstances(type).ToList();

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

    public static ValidationStates ExecuteEvent<TEntity, TEvent>(this IProcessor processor, TEntity entity, TEvent @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TEvent : class, IEvent
    {

        var dynamicEntity = (dynamic)entity;
        var dynamicEvent = (dynamic)@event;

        var validations = new ValidationStates();

        if (shouldValidate)
        {
            var validators = processor.Container.GetInstances<IPreExecuteValidator<TEntity, TEvent>>();

            foreach (var preExecuteValidator in validators)
            {
                if (preExecuteValidator.ShouldValidate(dynamicEntity, dynamicEvent))
                {
                    validations.Add(preExecuteValidator.Validate(dynamicEntity, dynamicEvent));
                }
            }
        }

        if (validations.IsValid)
        {
            var handler = processor.Container.GetInstance<IEventHandler<TEntity, TEvent>>();

            handler.Handle(dynamicEntity, dynamicEvent);
        }

        return validations;
    }
}