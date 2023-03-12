using System.Text;
using System.Text.Json;
using Mcc.Di;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Mcc.ServiceBus.RabbitMq;

public class RabbitMqEventReceiver : IEventReceiver
{

    private List<Func<EventReceivedEventArgs,Task>> actions = new();

    public RabbitMqEventReceiver(
        RabbitMqChannelFactory factory, 
        IOptions<RabbitMqOptions> options, 
        IDependencyContainer container, 
        ILogger<RabbitMqEventReceiver> logger)
    {
        var channel = factory.Create();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            logger.LogInformation("Received message");

            try
            {
                using (container.CreateScope())
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var result = JsonSerializer.Deserialize<Message>(message);

                    var args = new EventReceivedEventArgs { Message = result };

                    Task.WaitAll(actions.Select(action => action.Invoke(args)).ToArray());

                    channel.BasicAck(ea.DeliveryTag, false);

                    logger.LogInformation("Message of type '{0}' handled successful", result.Metadata.TypeName);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        foreach (var queue in options.Value.Queues ?? new List<RabbitMqQueue>())
        {
            channel.BasicConsume(queue: queue.Name, autoAck: false, consumer: consumer);
        }
    }


    public void Subscibe(Func<EventReceivedEventArgs, Task> action)
    {
        actions.Add(action);
    }
}