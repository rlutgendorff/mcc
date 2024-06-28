
using Mcc.Cqrs.Events;
using Mcc.EventSourcing.ServiceBus;

public class Message
{
    public string Data { get; set; }
    public EventMetadata Metadata { get; set; }
}