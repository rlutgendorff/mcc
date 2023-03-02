using Mcc.Cqrs.Commands;
using Mcc.Ddd;

namespace Mcc.EventSourcing.Cqrs;

public interface IEventHandler<in TEntity, in TCommand>
    where TEntity : class, IAggregate
    where TCommand : class, ICommand
{
    public void Handle(TEntity entity, TCommand @event);
}