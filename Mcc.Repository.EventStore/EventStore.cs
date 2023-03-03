using System.Text;
using System.Text.Json;
using EventStore.Client;
using Mcc.Cqrs.Commands;
using Mcc.EventSourcing;
using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Stores;
using Mcc.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mcc.Repository.EventStore;

public class EventStore : IEventStore
{
    private readonly EventStoreClient _eventStore;
    private readonly ILogger _logger;
    private readonly ITypeConverter _converter;

    public EventStore(EventStoreClientFactory eventStoreClientFactory, ILoggerFactory loggerFactory, ITypeConverter converter, IConfiguration configuration)
    {
        _eventStore = eventStoreClientFactory.Create(configuration);
        _converter = converter;
        _logger = loggerFactory.CreateLogger<EventStore>();
    }

    public async Task<IEnumerable<EventWrapper>> ReadEventsAsync(AggregateId id, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Read Events for {id}", id);

        var ret = new List<EventWrapper>();

        var result = await _eventStore.ReadStreamAsync(Direction.Forwards, id.ToString(), StreamPosition.Start, 200, cancellationToken: cancellationToken)
            .ToListAsync(cancellationToken);

        foreach (var resolvedEvent in result)
        {
            var wrapper = CreateWrapper(resolvedEvent);

            ret.Add(wrapper);
        }

        return ret;
    }

    public async Task<Queue<EventWrapper>> ReadAllStreams(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Read all Events");

        var ret = new Queue<EventWrapper>();

        var events = await _eventStore.ReadAllAsync(Direction.Forwards, Position.Start, cancellationToken: cancellationToken).ToListAsync(cancellationToken);

        foreach (var resolvedEvent in events)
        {
            using (_logger.BeginScope(("TraceID", Guid.NewGuid().ToString())))
            {
                _logger.LogDebug($"ResolvedEvent: {resolvedEvent.ToJson()}");

                if (resolvedEvent.Event.EventType.StartsWith('$')) continue;

                var wrapper = CreateWrapper(resolvedEvent);

                ret.Enqueue(wrapper);

                _logger.LogDebug("Added wrapper to queue");
                _logger.LogDebug($"Wrapper: {wrapper.ToJson()}");
            }
        }

        return ret;
    }

    public async Task AppendEventAsync(EventWrapper @event, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(("object", @event.ToJson())))
        {
            _logger.LogDebug("Append to stream {id}", @event.AggregateId);

            var eventId = Uuid.NewUuid();

            var eventData = new EventData(
                eventId,
                @event.Event.GetType().AssemblyQualifiedName,
                Serialize(@event.Event),
                Encoding.UTF8.GetBytes(@event.Metadata.Metadata.ToJson()));

            await _eventStore.AppendToStreamAsync(
                @event.AggregateId.ToString(),
                @event.AggregateVersion ?? StreamRevision.None,
                new[] { eventData },
                cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteAsync(AggregateId id, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Delete Stream with id: {id}", id);

        await _eventStore.TombstoneAsync(id.ToString(), StreamState.Any, cancellationToken: cancellationToken);
    }

    private EventWrapper CreateWrapper(ResolvedEvent resolvedEvent)
    {
        var @event = Deserialize(resolvedEvent.Event.EventType, resolvedEvent.Event.Data.ToArray());

        var aggregateId = new AggregateId(resolvedEvent.OriginalStreamId);

        return new EventWrapper
        {
            Event = @event,
            EventId = resolvedEvent.Event.EventId.ToGuid(),
            AggregateId = aggregateId,
            AggregateVersion = resolvedEvent.Event.EventNumber.ToUInt64(),
            Metadata = new EventMetadata
            {
                Id = aggregateId.Id,
                Metadata = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.ToArray()))!
            }
        };
    }

    private ICommand Deserialize(string eventType, byte[] data)
    {
        var utf8Data = Encoding.UTF8.GetString(data);
        var type = _converter.CreateType(eventType);

        using (_logger.BeginScope(new Dictionary<string, object>
               {
                   ["Data"] = utf8Data,
                   ["EventType"] = eventType,
               }))
        {
            _logger.LogDebug("Try to deserialize data");

            var result = (ICommand)JsonSerializer.Deserialize(utf8Data, type)!;

            _logger.LogDebug("Deserialize successful");

            return result;
        }
    }

    private byte[] Serialize(ICommand @event)
    {
        return Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize((dynamic)@event));
    }
}