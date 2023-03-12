using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Cqrs;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing;

public class EventWrapper
{
    public EventWrapper()
    {
        EventId = Guid.NewGuid();
    }

    public Guid EventId { get; set; }

    public AggregateId AggregateId { get; set; }

    public ulong? AggregateVersion { get; set; }

    public IEvent Event { get; set; }

    public required EventMetadata Metadata { get; set; }
}