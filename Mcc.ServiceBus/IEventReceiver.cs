namespace Mcc.ServiceBus;

public interface IEventReceiver
{
    void Subscibe(Func<EventReceivedEventArgs, Task> action);
}

public class EventReceivedEventArgs : EventArgs
{
    public Message Message { get; set; }
}
