using Mcc.Ddd;
using Mcc.EventSourcing.Aggregates;

namespace Mcc.EventSourcing.Extensions;

public static class AggregateExtensions
{
    public static AggregateId GetId<TAggregate>(this TAggregate aggregate)
        where TAggregate : IAggregate
    {
        return new AggregateId(typeof(TAggregate), aggregate.Id);
    }
}