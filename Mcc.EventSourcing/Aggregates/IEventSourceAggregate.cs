using Mcc.Ddd;

namespace Mcc.EventSourcing.Aggregates;

public interface IEventSourceAggregate : IAggregate
{
    ulong? Version { get; }
}