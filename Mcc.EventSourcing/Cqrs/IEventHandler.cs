using Mcc.Cqrs.Events;
using Mcc.Ddd;

namespace Mcc.EventSourcing.Cqrs;

public interface IEventHandler<in TEntity, in TEvent>
    where TEntity : class, IAggregate
    where TEvent : class, IEvent
{
    public void Handle(TEntity entity, TEvent @event);
}