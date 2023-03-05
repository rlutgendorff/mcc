namespace Mcc.ServiceBus;

public class EventMetadata
{
    public Guid Id { get; set; }
    public required string TypeName { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
}