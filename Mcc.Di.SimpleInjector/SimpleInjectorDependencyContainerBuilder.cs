using Mcc.Di.SimpleInjector.Extensions;
using SimpleInjector.Lifestyles;
using SimpleInjector;

namespace Mcc.Di.SimpleInjector;

public class SimpleInjectorDependencyContainerBuilder : IDependencyContainerCombinedBuilder
{
    private Container _container;

    public SimpleInjectorDependencyContainerBuilder()
    {
        _container = new Container();
        _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
    }

    public IDependencyContainer Build()
    {
        _container.Verify();

        var container = new SimpleInjectorDependencyContainer(_container);

        return container;
    }

    public void Register<TService, TImplementation>(DependencyLifestyle lifestyle)
        where TService : class
        where TImplementation : class, TService
    {
        _container.Register<TService, TImplementation>(lifestyle.GetLifestyle());
    }
}