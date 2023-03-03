namespace Mcc.EventSourcing.Aggregates.Services;

public class AggregateEventsService
{
    public IEnumerable<EventWrapper> GetUncommittedEvents(BaseEventSourceAggregate aggregate)
    {
        return aggregate.ChangeTracker.GetUncommittedEvents();
    }

    public void ClearUncommittedEvents(BaseEventSourceAggregate aggregate)
    {
        aggregate.ChangeTracker.ClearUncommittedEvents();
    }

    public void Apply(BaseEventSourceAggregate aggregate, EventWrapper @event, bool shouldValidate)
    {
        aggregate.ChangeTracker.Apply(@event, shouldValidate);
    }
}