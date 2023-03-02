using Mcc.Cqrs.Commands;
using Mcc.Cqrs.Queries;

namespace Mcc.Cqrs;

public interface IProcessor
{
    Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

    Task<ICommandResponse<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);
}