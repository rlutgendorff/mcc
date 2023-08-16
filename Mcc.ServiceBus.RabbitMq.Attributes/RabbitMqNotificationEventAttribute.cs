namespace Mcc.ServiceBus.RabbitMq.Attributes;

[System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RabbitMqNotificationEventAttribute : Attribute
{
    public string ExchangeName { get; set; }
}