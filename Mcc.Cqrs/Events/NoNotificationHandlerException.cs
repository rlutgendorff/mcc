namespace Mcc.Cqrs.Events;

public class NoNotificationHandlerException : ApplicationException
{
    public NoNotificationHandlerException() { }
    public NoNotificationHandlerException(string message) : base(message) { }
}