using Mcc.EventSourcing.Stores;
using Mcc.Repository.Databases;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Projections;

public class ProjectionRebuilder<TEntity> : IProjectionRebuilder<TEntity>
{
    private readonly IDatabaseRepository<TEntity> _repository;
    private readonly IEventStore _eventStore;

    public ProjectionRebuilder(IDatabaseRepository<TEntity> repository, IEventStore eventStore)
    {
        _repository = repository;
        _eventStore = eventStore;
    }

    public async Task RebuildAsync(Func<EventWrapper, Task> action, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(_repository.GetAll(), cancellationToken);

        var events = await _eventStore.ReadAllStreams(cancellationToken);

        foreach (var @event in events)
        {
            await action(@event);
        }
    }
}