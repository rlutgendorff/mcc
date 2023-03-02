using Mcc.Cqrs.Commands;
using Mcc.EventSourcing.Aggregates;

namespace Mcc.EventSourcing;

public class EventWrapper
{
    public EventWrapper()
    {
        EventId = Guid.NewGuid();
        Metadata = new();
    }

    public Guid EventId { get; set; }

    public AggregateId AggregateId { get; set; }

    public ulong? AggregateVersion { get; set; }

    public ICommand Event { get; set; }

    public EventMetadata Metadata { get; set; }
}