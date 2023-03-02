namespace Mcc.EventSourcing;

public class EventMetadata
{
    public Guid Id { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
}