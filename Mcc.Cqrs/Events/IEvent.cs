using Mcc.Cqrs.Commands;

namespace Mcc.Cqrs.Events;

public interface IEvent : ICommand { }

public interface IEvent<TResult> : IEvent, ICommand<TResult> { }