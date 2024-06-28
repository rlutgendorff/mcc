using System.Reflection;
using Mcc.Cqrs.Events;
using Mcc.EventSourcing.Cqrs;
using Mcc.ServiceBus.RabbitMq.Attributes;
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

        var exchanges = GetExchanges();

        foreach (var exchange in exchanges)
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, true);
        }

        var queueBindings = GetQueueBindings();

        var queueName = Assembly.GetEntryAssembly().GetName().Name;

        _channel.QueueDeclare(queueName, true, false, autoDelete: false);

        foreach (var queueBinding in queueBindings)
        {
            _channel.QueueBind(queueName, queueBinding.exchange, queueBinding.typeName);
        }
        
    }

    public IModel Create() => _channel;

    private List<string> GetExchanges()
    {
        var types = GetImplementingTypes(typeof(IEvent));

        return types
            .Select(x =>
                x.GetCustomAttribute<RabbitMqNotificationEventAttribute>()?.ExchangeName)
            .Where(x=> x != null)
            .Distinct()
            .ToList()!;
    }

    private IEnumerable<(string exchange, string typeName)> GetQueueBindings()
    {
        var types = GetImplementingTypes(typeof(INotificationHandler<>));

        foreach (var type in types)
        {
            foreach (var @interface in type.GetInterfaces().Where(x=> x.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
            {
                var genericType = @interface.GenericTypeArguments[0];

                var exchangeName = genericType.GetCustomAttribute<RabbitMqNotificationEventAttribute>()?.ExchangeName;

                if (exchangeName == null)
                    continue;

                yield return new(
                    exchangeName,
                    genericType.Name
                );
            }
        }
    }

    

    static List<Type> GetImplementingTypes(Type interfaceType)
    {
        List<Type> implementingTypes = new List<Type>();

        // Iterate through all types in the executing assembly and its referenced assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types = assembly.GetTypes();

            // Filter out types that don't implement the interface
            foreach (var type in types)
            {
                if (type.IsClass && type.GetInterfaces().Contains(interfaceType))
                {
                    implementingTypes.Add(type);
                }
                if (type.IsClass && type.GetInterfaces().Any(x=>x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType))
                {
                    implementingTypes.Add(type);
                }
            }
        }

        return implementingTypes;
    }
}