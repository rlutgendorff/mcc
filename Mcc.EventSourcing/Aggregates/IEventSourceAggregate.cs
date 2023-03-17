using Mcc.Ddd;

namespace Mcc.EventSourcing.Aggregates;

public interface IEventSourceAggregate : IAggregate
{
    long? Version { get; }
}