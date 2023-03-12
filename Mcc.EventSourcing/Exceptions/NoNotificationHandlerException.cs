namespace Mcc.EventSourcing.Exceptions;

public class NoNotificationHandlerException : ApplicationException
{
    public NoNotificationHandlerException() { }
    public NoNotificationHandlerException(string message) : base(message) { }
}