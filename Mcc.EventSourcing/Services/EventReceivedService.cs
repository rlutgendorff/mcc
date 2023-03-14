using Mcc.EventSourcing.Cqrs.Processors;
using System.Text.Json;
using Mcc.EventSourcing.Cqrs;
using Mcc.Di;
using Mcc.EventSourcing.ServiceBus;

namespace Mcc.EventSourcing.Services;

public class EventReceivedService
{
    private readonly IEventSourcingProcessor _processor;
    private readonly ITypeConverter _converter;

    public EventReceivedService(IEventReceiver receiver, IEventSourcingProcessor processor, ITypeConverter converter)
    {
        _processor = processor;
        _converter = converter;
        receiver.Subscibe(Receiver_EventReceived);
    }

    private Task Receiver_EventReceived(EventReceivedEventArgs e)
    {
        var type = _converter.CreateType(e.Message.Metadata.TypeName);

        var command = (IEvent)JsonSerializer.Deserialize(e.Message.Data, type)!;

        return _processor.Notify(command, CancellationToken.None, e.Message.Metadata);
    }
}