using Mcc.Cqrs.Commands;
using Mcc.Cqrs.Events;
using Mcc.Cqrs.Queries;
using Mcc.Di;
using System.Diagnostics;
using Mcc.Cqrs.Events.Validations;
using Mcc.Ddd;

namespace Mcc.Cqrs;

public class Processor : IProcessor
{
    public Processor(IDependencyContainer container)
    {
        Container = container;
    }

    public IDependencyContainer Container { get; }

    public Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        dynamic handler = Container.GetInstance(handlerType);
        return handler.HandleAsync((dynamic)query, cancellationToken);
    }

    public Task<ICommandResponse<TResult>> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = Container.GetInstance(handlerType);
        return handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public Task Notify(IEvent command, EventMetadata metadata, CancellationToken cancellationToken)
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