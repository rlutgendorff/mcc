using System.Data;
using Marten;
using Marten.Services;
using Mcc.Ddd;
using Mcc.EventSourcing.Aggregates;
using Mcc.EventSourcing.Snapshots;

namespace Mcc.Repository.MartenDb;

public class MartenDbSnapshotService<TAggregate> : ISnapshotService<TAggregate>
    where TAggregate : BaseEventSourceAggregate
{
    private readonly IDocumentSession _session;

    public MartenDbSnapshotService(IDocumentStore store)
    {
        _session = store.OpenSession(DocumentTracking.None);
    }

    public Task Store(TAggregate aggregate, CancellationToken cancellationToken)
    {
        _session.Store(aggregate);
        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task<TAggregate?> Restore(Guid aggregateId, CancellationToken cancellationToken)
    {
        return _session.LoadAsync<TAggregate>(aggregateId, cancellationToken);
    }
}