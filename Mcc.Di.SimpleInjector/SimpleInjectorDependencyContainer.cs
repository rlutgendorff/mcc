using SimpleInjector;

namespace Mcc.Di.SimpleInjector;

public class DependencyContainer : IDependencyContainer
{
    private readonly Container _container;

    public DependencyContainer(Container container)
    {
        _container = container;
    }

    public TType GetInstance<TType>()
        where TType : class
    {
        return _container.GetInstance<TType>();
    }

    public object GetInstance(Type type)
    {
        return _container.GetInstance(type);
    }

    public IEnumerable<TType> GetInstances<TType>()
        where TType : class
    {
        return _container.GetAllInstances<TType>();
    }

    public IEnumerable<object> GetInstances(Type type)
    {
        return _container.GetAllInstances(type);
    }
}