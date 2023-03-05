using System.Text;
using Mcc.Extensions;
using RabbitMQ.Client;

namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqEventPublisher : IEventPublisher
{
    public readonly IModel _channel;

    public RabbitMqEventPublisher(RabbitMqChannelFactory factory)
    {
        _channel = factory.Create();
    }

    public void Publish(string exchange, Message obj)
    {
        var message = Encoding.UTF8.GetBytes(obj.ToJson());

        _channel.BasicPublish(exchange, obj.Data.GetType().Name, null, message);
    }
}