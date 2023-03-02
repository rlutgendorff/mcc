namespace Mcc.Cqrs.Commands;

public interface ICommand { }

public interface ICommand<TResult> : ICommand
{
}