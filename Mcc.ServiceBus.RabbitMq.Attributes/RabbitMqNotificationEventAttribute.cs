namespace Mcc.ServiceBus.RabbitMq.Attributes;

[System.AttributeUsage(AttributeTargets.Class)]
public class RabbitMqNotificationEventAttribute : Attribute
{
    public string ExchangeName { get; set; }
}