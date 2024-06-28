using Mcc.Cqrs.Events.Validations;
using Mcc.Ddd;

namespace Mcc.Cqrs.Events;

public interface IPreExecuteValidator<in TEntity, in TEvent>
    where TEntity : IAggregate
    where TEvent : IEvent
{
    bool ShouldValidate(TEntity entity, TEvent @event);
    ValidationState Validate(TEntity entity, TEvent @event);
}