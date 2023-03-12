using System.Data;
using Marten;
using Marten.Events;
using Marten.Services;
using Mcc.EventSourcing;
using Mcc.EventSourcing.Aggregates;
using Mcc.ServiceBus;
using IEventStore = Mcc.EventSourcing.Stores.IEventStore;

namespace Mcc.Repository.MartenDb;

public class MartenDbEventStore : IEventStore
{
    private readonly IDocumentSession _session;

    public MartenDbEventStore(IDocumentStore store)
    {
        _session = store.LightweightSession();
    }

    public async Task<IEnumerable<EventWrapper>> ReadEventsAsync(AggregateId id, CancellationToken cancellationToken)
    {
        var events = await _session.Events.FetchStreamAsync(id.Id, token: cancellationToken);

        var result = new List<EventWrapper>();

        foreach (var @event in events)
        {
            var wrapper = CreateWrapper(@event);

            result.Add(wrapper);
        }

        return result;
    }

    public Task AppendEventAsync(EventWrapper @event, CancellationToken cancellationToken)
    {

        if (@event.AggregateVersion != null)
        {
            _session.Events.Append(@event.AggregateId.Id, (long)@event.AggregateVersion , @event.Event);
        }
        else
        {
            _session.Events.StartStream(@event.AggregateId.Id, @event.Event);
        }

        
        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task<Queue<EventWrapper>> ReadAllStreams(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var events = _session.Events.QueryAllRawEvents();
            var result = new Queue<EventWrapper>();

            foreach (var @event in events)
            {
                if (@event.EventTypeName is "tombstone") continue;

                var wrapper = CreateWrapper(@event);

                result.Enqueue(wrapper);
            }

            return result;
        }, cancellationToken);
    }

    public Task DeleteAsync(AggregateId id, CancellationToken cancellationToken)
    {
        return Task.Run(()=>_session.Delete(id.Id), cancellationToken);
    }

    private EventWrapper CreateWrapper(IEvent @event)
    {
        

        var aggregateId = new AggregateId(@event.EventType, @event.StreamId);

        var wrapper = new EventWrapper
        {
            AggregateId = aggregateId,
            AggregateVersion = (ulong)@event.Version,
            Event = (EventSourcing.Cqrs.IEvent)@event.Data,
            EventId = @event.Id,
            Metadata = new EventMetadata
            {
                Id = @event.StreamId,
                TypeName = @event.EventType.AssemblyQualifiedName,
                Metadata = new Dictionary<string, string>()
            }
        };

        return wrapper;
    }
}