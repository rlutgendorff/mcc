namespace Mcc.ServiceBus;

public interface IEventPublisher
{
    public void Publish(string exchange, string routingKey, Message obj);
}