using Mcc.Ddd;
using Mcc.EventSourcing.Cqrs;
using Mcc.EventSourcing.Validations;
using Mcc.Cqrs;
using Mcc.Cqrs.Events;

namespace Mcc.EventSourcing.Extensions;

public static class ProcessorExtensions
{
    public static ValidationStates ExecuteEvent<TEntity, TEvent>(this IProcessor processor, TEntity entity, TEvent @event, bool shouldValidate)
        where TEntity : class, IAggregate
        where TEvent : class, IEvent
    {

        var dynamicEntity = (dynamic)entity;
        var dynamicEvent = (dynamic)@event;

        var validations = new ValidationStates();

        if (shouldValidate)
        {
            var validators = processor.Container.GetInstances<IPreExecuteValidator<TEntity, TEvent>>();

            foreach (var preExecuteValidator in validators)
            {
                if (preExecuteValidator.ShouldValidate(dynamicEntity, dynamicEvent))
                {
                    validations.Add(preExecuteValidator.Validate(dynamicEntity, dynamicEvent));
                }
            }
        }

        if (validations.IsValid)
        {
            var handler = processor.Container.GetInstance<IEventHandler<TEntity, TEvent>>();

            handler.Handle(dynamicEntity, dynamicEvent);
        }

        return validations;
    }
}