namespace Mcc.Clocks;

public interface IClock
{
    DateTime UtcNow { get; }
}

public class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}