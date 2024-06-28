using Mcc.Cqrs.Commands;
using Mcc.Cqrs.Queries;
using Mcc.Di;

namespace Mcc.Cqrs;

public interface IProcessor
{
    public IDependencyContainer Container { get; }

    Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

    Task<ICommandResponse<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);
}