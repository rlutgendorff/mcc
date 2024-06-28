using System.Text.Json;
using Mcc.Cqrs;
using Mcc.Cqrs.Events;
using Mcc.EventSourcing.Cqrs;
using Mcc.Di;
using Mcc.ServiceBus;

namespace Mcc.EventSourcing.Services;

public class EventReceivedService
{
    private readonly IProcessor _processor;

    public EventReceivedService(IEventReceiver receiver, IProcessor processor)
    {
        _processor = processor;
        receiver.Subscibe(Receiver_EventReceived);
    }

    private Task Receiver_EventReceived(EventReceivedEventArgs e)
    {
        var type = Type.GetType(e.Message.Metadata.TypeName);

        var command = (IEvent)JsonSerializer.Deserialize(e.Message.Data, type!)!;

        return _processor.Notify(command, e.Message.Metadata, CancellationToken.None);
    }
}