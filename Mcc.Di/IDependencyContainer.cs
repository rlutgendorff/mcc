namespace Mcc.Di;

public interface IDependencyContainer
{
    TType GetInstance<TType>() where TType : class;
    object GetInstance(Type type);

    IEnumerable<TType> GetInstances<TType>() where TType : class;

    IEnumerable<object> GetInstances(Type type);

    IDependencyScope CreateScope();
}