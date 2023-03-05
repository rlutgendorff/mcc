namespace Mcc.ServiceBus;

public interface IEventPublisher
{
    public void Publish(string exchange, Message obj);
}