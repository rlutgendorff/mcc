using Mcc.Cqrs.Events;

namespace Mcc.ServiceBus;

public class Message
{
    public string Data { get; set; }
    public EventMetadata Metadata { get; set; }
}