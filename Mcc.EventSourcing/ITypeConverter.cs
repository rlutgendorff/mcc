namespace Mcc.EventSourcing;

public interface ITypeConverter
{
    Type? CreateType(string type);
}