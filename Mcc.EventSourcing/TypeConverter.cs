namespace Mcc.EventSourcing;

public class TypeConverter : ITypeConverter
{
    public Type? CreateType(string type)
    {
        var createdType = Type.GetType(type);

        return createdType;
    }
}