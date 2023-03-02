namespace Mcc.EventSourcing.Validations;

public class ValidationState
{
    private readonly List<Error> _errors = new();

    public bool IsValid => !Errors.Any();

    public IReadOnlyList<Error> Errors => _errors;

    public void AddError(string paramName, string message)
    {
        _errors.Add(new Error
        {
            ParamName = paramName,
            Message = message,
        });
    }
}