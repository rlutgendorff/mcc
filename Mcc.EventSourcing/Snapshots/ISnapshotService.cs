using Mcc.Ddd;
using Mcc.EventSourcing.Aggregates;

namespace Mcc.EventSourcing.Snapshots;

public interface ISnapshotService<TAggregate>
    where TAggregate : BaseEventSourceAggregate
{
    Task Store(TAggregate aggregate, CancellationToken cancellationToken);

    Task<TAggregate?> Restore(Guid aggregateId, CancellationToken cancellationToken);
}