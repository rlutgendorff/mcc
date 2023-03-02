using Mcc.Cqrs.Queries;
using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Stores;

namespace Mcc.EventSourcing.Cqrs.Queries;

public sealed record LoadAggregateEventsQuery : IQuery<IEnumerable<EventWrapper>>
{
    public AggregateId AggregateId { get; set; }
}

public sealed class LoadAggregateEventsQueryHandler : IQueryHandler<LoadAggregateEventsQuery, IEnumerable<EventWrapper>>
{
    private readonly IEventStore _eventStore;

    public LoadAggregateEventsQueryHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public Task<IEnumerable<EventWrapper>> HandleAsync(LoadAggregateEventsQuery query, CancellationToken cancellationToken)
    {
        return _eventStore.ReadEventsAsync(query.AggregateId, cancellationToken);
    }
}