using Mcc.Cqrs.Commands;
using Mcc.Ddd;

namespace Mcc.EventSourcing.Cqrs.Commands;

public class DeleteNotification : IEvent
{
    public Guid Id { get; set; }
}

public class DeleteNotification<TEntity> : DeleteNotification, ICommand
    where TEntity : BaseAggregate
{ }