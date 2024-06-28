
namespace Mcc.ServiceBus;

public interface IEventPublisher
{
    public void Publish(EventWrapper eventWrapper);
}