using Mcc.Ddd;
using Mcc.EventSourcing.Aggregates;

namespace Mcc.EventSourcing.Snapshots;

public class DisabledSnapshotService<TAggregate> : ISnapshotService<TAggregate>
    where TAggregate : BaseEventSourceAggregate
{
    public Task Store(TAggregate aggregate, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<TAggregate?> Restore(Guid aggregateId, CancellationToken cancellationToken)
    {
        return Task.FromResult<TAggregate?>(null);
    }
}