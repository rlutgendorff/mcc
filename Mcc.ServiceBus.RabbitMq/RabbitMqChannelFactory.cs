using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqChannelFactory
{
    private readonly IModel _channel;

    public RabbitMqChannelFactory(IOptions<RabbitMqOptions> options)
    {
        var settings = options.Value;

        var factory = new ConnectionFactory()
            { HostName = settings.HostName, UserName = settings.Username, Password = settings.Password };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        foreach (var exchange in settings.Exchanges)
        {
            _channel.ExchangeDeclare(exchange.Name, exchange.Type, true);
        }

        foreach (var queue in settings.Queues)
        {
            _channel.QueueDeclare(queue.Name, true, false, autoDelete: false);

            foreach (var binding in queue.Bindings)
            {
                _channel.QueueBind(queue.Name, binding.Exchange, binding.RoutingKey);
            }
        }
    }

    public IModel Create() => _channel;
}