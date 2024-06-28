namespace Mcc.Cqrs.Events.Validations;

public class ValidationStates
{
    private readonly List<ValidationState> _validations = new();

    public bool IsValid => _validations.All(v => v.IsValid);
    public IReadOnlyList<Error> Errors => _validations.SelectMany(v => v.Errors).ToList();

    public void Add(ValidationState validation)
    {
        _validations.Add(validation);
    }
}