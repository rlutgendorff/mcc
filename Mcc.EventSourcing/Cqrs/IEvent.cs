using Mcc.Cqrs.Commands;

namespace Mcc.EventSourcing.Cqrs;

public interface IEvent : ICommand { }

public interface IEvent<TResult> : ICommand<TResult> { }