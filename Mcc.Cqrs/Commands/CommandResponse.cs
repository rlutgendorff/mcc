namespace Mcc.Cqrs.Commands;

public record CommandResponse<TEntity> : ICommandResponse<TEntity>
{
    public virtual CommandResultType ResultType { get; init; }
    public virtual Exception? Exception { get; init; }
    public TEntity? Result { get; init; }
}