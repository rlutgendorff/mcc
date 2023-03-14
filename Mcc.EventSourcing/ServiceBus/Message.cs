namespace Mcc.EventSourcing.ServiceBus;

public class Message
{
    public string Data { get; set; }
    public EventSourcing.ServiceBus.EventMetadata Metadata { get; set; }
}