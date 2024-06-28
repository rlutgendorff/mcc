using Mcc.Cqrs.Events;

namespace Mcc.ServiceBus;

public class EventWrapper
{
    public EventWrapper()
    {
        EventId = Guid.NewGuid();
    }

    public Guid EventId { get; set; }

    public Guid AggregateId { get; set; }

    public long AggregateVersion { get; set; }

    public IEvent Event { get; set; }

    public required EventMetadata Metadata { get; set; }
}