namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqOptions
{
    public const string Key = "rabbitmq";
    public string HostName { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
}