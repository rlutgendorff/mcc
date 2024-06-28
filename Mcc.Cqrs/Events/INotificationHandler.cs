namespace Mcc.Cqrs.Events;

public interface INotificationHandler<in TEvent>
    where TEvent : IEvent
{
    Task HandleAsync(TEvent command, CancellationToken cancellationToken, EventMetadata metadata);
}