using System.Reflection;
using System.Text;
using Mcc.EventSourcing;
using Mcc.EventSourcing.Cqrs;
using Mcc.EventSourcing.ServiceBus;
using Mcc.Extensions;
using Mcc.ServiceBus.RabbitMq.Attributes;
using RabbitMQ.Client;

namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqEventPublisher : IEventPublisher
{
    public readonly IModel _channel;

    public RabbitMqEventPublisher(RabbitMqChannelFactory factory)
    {
        _channel = factory.Create();
    }

    public void Publish(EventWrapper eventWrapper)
    {
        var exchange = GetExchange(eventWrapper.Event);

        if (exchange != null)
        {
            var message = new Message
            {
                Data = eventWrapper.Event.ToJson(),
                Metadata = eventWrapper.Metadata
            };

            var body = Encoding.UTF8.GetBytes(message.ToJson());

            _channel.BasicPublish(exchange, eventWrapper.Event.GetType().Name, null, body);
        }
    }

    public string? GetExchange(IEvent @event)
    {
        var attribute = @event.GetType().GetCustomAttribute(typeof(RabbitMqNotificationEventAttribute),true) as RabbitMqNotificationEventAttribute;

        return attribute?.ExchangeName;
    }
}