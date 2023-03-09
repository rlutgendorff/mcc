namespace Mcc.Di.SimpleInjector;

public class DependencyScope : IDependencyScope
{
    private readonly global::SimpleInjector.Scope _scope;

    public DependencyScope(global::SimpleInjector.Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}