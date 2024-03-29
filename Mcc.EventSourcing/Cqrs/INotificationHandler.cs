﻿using Mcc.EventSourcing.ServiceBus;

namespace Mcc.EventSourcing.Cqrs;

public interface INotificationHandler<in TEvent>
    where TEvent : IEvent
{
    Task HandleAsync(TEvent command, CancellationToken cancellationToken, EventMetadata metadata);
}