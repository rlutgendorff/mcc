using Mcc.Cqrs.Commands;
using Mcc.Ddd;
using Mcc.EventSourcing.Validations;

namespace Mcc.EventSourcing.Cqrs;

public interface IPreExecuteValidator<in TEntity, in TCommand>
    where TEntity : IAggregate
    where TCommand : ICommand
{
    bool ShouldValidate(TEntity entity, TCommand @event);
    ValidationState Validate(TEntity entity, TCommand @event);
}