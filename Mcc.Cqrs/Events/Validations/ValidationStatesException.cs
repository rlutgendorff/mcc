using System.Text;

namespace Mcc.Cqrs.Events.Validations;

public class ValidationStatesException : ApplicationException
{
    public ValidationStatesException(ValidationStates validationStates)
    {
        ValidationStates = validationStates;
    }

    public ValidationStates ValidationStates { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Validation Failed on the following errors:");

        foreach (var validationStatesError in ValidationStates.Errors)
        {
            sb.Append(validationStatesError);
        }

        return sb.ToString();
    }
}