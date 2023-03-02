using Mcc.Cqrs.Commands;

namespace Mcc.EventSourcing.Cqrs;

public interface INotificationHandler<in TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken, EventMetadata metadata);
}