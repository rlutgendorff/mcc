using Mcc.Cqrs.Events;
using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Cqrs;
using Mcc.EventSourcing.ServiceBus;

namespace Mcc.EventSourcing;

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