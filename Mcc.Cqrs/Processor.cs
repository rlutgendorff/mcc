using Mcc.Cqrs.Commands;
using Mcc.Cqrs.Queries;
using Mcc.Di;

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
}