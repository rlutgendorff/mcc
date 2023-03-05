using Mcc.Cqrs.Commands;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Cqrs;

public interface INotificationHandler<in TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken, EventMetadata metadata);
}