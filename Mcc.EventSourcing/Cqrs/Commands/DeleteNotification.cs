using Mcc.Ddd;
using MediatR;

namespace Mcc.EventSourcing.Cqrs.Commands;

public class DeleteNotification : INotification
{
    public Guid Id { get; set; }
}

public class DeleteNotification<TEntity> : DeleteNotification
    where TEntity : BaseAggregate
{ }