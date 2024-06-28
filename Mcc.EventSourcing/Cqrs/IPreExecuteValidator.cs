using Mcc.Cqrs.Events;
using Mcc.Ddd;
using Mcc.EventSourcing.Validations;

namespace Mcc.EventSourcing.Cqrs;

public interface IPreExecuteValidator<in TEntity, in TEvent>
    where TEntity : IAggregate
    where TEvent : IEvent
{
    bool ShouldValidate(TEntity entity, TEvent @event);
    ValidationState Validate(TEntity entity, TEvent @event);
}