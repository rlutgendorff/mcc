using Mcc.Cqrs.Commands;
using Mcc.EventSourcing.Cqrs.Processors;
using Mcc.ServiceBus;
using System.Text.Json;

namespace Mcc.EventSourcing.Services;

public class EventReceivedService
{
    private readonly IEventSourcingProcessor _processor;
    private readonly ITypeConverter _converter;

    public EventReceivedService(IEventReceiver receiver, IEventSourcingProcessor processor, ITypeConverter converter)
    {
        _processor = processor;
        _converter = converter;
        receiver.EventReceived += Receiver_EventReceived;
    }

    private void Receiver_EventReceived(object? sender, EventReceivedEventArgs e)
    {
        var type = _converter.CreateType(e.Message.Metadata.TypeName);

        var command = (ICommand)JsonSerializer.Deserialize(e.Message.Data, type)!;

        _processor.Notify(command, CancellationToken.None, e.Message.Metadata);
    }
}