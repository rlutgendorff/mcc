namespace Mcc.EventSourcing.ServiceBus;

public interface IEventPublisher
{
    public void Publish(EventWrapper eventWrapper);
}