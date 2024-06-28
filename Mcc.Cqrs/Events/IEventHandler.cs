using Mcc.Ddd;

namespace Mcc.Cqrs.Events;

public interface IEventHandler<in TEntity, in TEvent>
    where TEntity : class, IAggregate
    where TEvent : class, IEvent
{
    public void Handle(TEntity entity, TEvent @event);
}