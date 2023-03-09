using SimpleInjector.Lifestyles;
using Si = SimpleInjector;

namespace Mcc.Di.SimpleInjector;

public class DependencyContainer : IDependencyContainer
{
    private readonly Si.Container _container;

    public DependencyContainer(Si.Container container)
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

    public IDependencyScope CreateScope()
    {
        var scope = AsyncScopedLifestyle.BeginScope(_container);
        return new DependencyScope(scope);
    }
}

public class DependencyScope : IDependencyScope
{
    private readonly Si.Scope _scope;

    public DependencyScope(Si.Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}