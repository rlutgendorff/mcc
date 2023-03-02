namespace Mcc.Ddd;

public abstract class BaseAggregate : IAggregate
{
    public Guid Id { get; set; }
}