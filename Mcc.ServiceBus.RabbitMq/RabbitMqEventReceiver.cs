﻿using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqEventReceiver : IEventReceiver
{
    private IModel _channel;

    public RabbitMqEventReceiver(RabbitMqChannelFactory factory, IOptions<RabbitMqOptions> options)
    {
        var channel = factory.Create();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var result = JsonSerializer.Deserialize<Message>(message);

            EventReceived?.Invoke(this, new EventReceivedEventArgs { Message = result });
        };

        foreach (var queue in options.Value.Queues)
        {
            channel.BasicConsume(queue: queue.Name, autoAck: true, consumer: consumer);
        }
    }

    public event EventHandler<EventReceivedEventArgs>? EventReceived;
}