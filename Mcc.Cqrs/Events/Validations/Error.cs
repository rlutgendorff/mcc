namespace Mcc.Cqrs.Events.Validations;

public class Error
{
    public Error()
    {
        ParamName = string.Empty;
        Message = string.Empty;
    }

    public string ParamName { get; set; }

    public string Message { get; set; }
}