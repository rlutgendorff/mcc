namespace Mcc.EventSourcing.Aggregates;

public class AggregateId
{
    public AggregateId(Type type, Guid id)
    {
        Type = type.Name;
        Id = id;
    }

    public AggregateId(string streamName)
    {
        Type = streamName.Split('-')[0];
        Id = Guid.Parse(streamName.Replace($"{Type}-", ""));
    }

    public string Type { get; set; }
    public Guid Id { get; set; }

    public override string ToString()
    {
        return $"{Type}-{Id}";
    }
}