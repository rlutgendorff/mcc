namespace Mcc.Di.SimpleInjector;

public class SimpleInjectorDependencyScope : IDependencyScope
{
    private readonly global::SimpleInjector.Scope _scope;

    public SimpleInjectorDependencyScope(global::SimpleInjector.Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}