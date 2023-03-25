using Mcc.Cqrs.Commands;
using Mcc.Ddd;

namespace Mcc.EventSourcing.Cqrs.Commands;

public class DeleteNotification : IEvent
{
    public Guid Id { get; set; }
}

public class DeleteNotification<TEntity> : DeleteNotification
    where TEntity : BaseAggregate
{ }