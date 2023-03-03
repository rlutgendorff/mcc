namespace Mcc.Repository.EventStore;

public interface ITypeConverter
{
    Type CreateType(string type);
}