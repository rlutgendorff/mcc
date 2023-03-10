namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqOptions
{
    public const string Key = "rabbitmq";
    public string HostName { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }

    public List<RabbitMqExchange>? Exchanges { get; set; }
    public List<RabbitMqQueue>? Queues { get; set; }
}

public class RabbitMqExchange
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class RabbitMqQueue
{
    public string Name { get; set; }
    public List<RabbitMqBinding> Bindings { get; set; }

}

public class RabbitMqBinding
{
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
}