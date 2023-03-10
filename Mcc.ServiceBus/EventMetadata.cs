namespace Mcc.ServiceBus;

public class EventMetadata
{
    public Guid Id { get; set; }
    public required string TypeName { get; init; }
    public Dictionary<string, string> Metadata { get; set; } = new();
}