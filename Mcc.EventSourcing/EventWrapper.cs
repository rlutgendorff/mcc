using Mcc.Cqrs.Commands;
using Mcc.EventSourcing.Aggregates;
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

    public ICommand Event { get; set; }

    public required EventMetadata Metadata { get; set; }
}