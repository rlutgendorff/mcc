namespace Mcc.ServiceBus;

public interface IEventReceiver
{
    event EventHandler<EventReceivedEventArgs> EventReceived;
}

public class EventReceivedEventArgs : EventArgs
{
    public Message Message { get; set; }
}
