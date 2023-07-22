namespace Mcc.Di;

public interface IDependencyContainerBuilder
{
    IDependencyContainer Build();
}

public interface IDependencyContainerTypeBuilder : IDependencyContainerBuilder
{

}

public interface IDependencyContainerGenericBuilder : IDependencyContainerBuilder
{
    void Register<TService, TImplementation>(DependencyLifestyle lifestyle)
        where TService : class
        where TImplementation : class, TService;
}

public interface IDependencyContainerCombinedBuilder : IDependencyContainerTypeBuilder, IDependencyContainerGenericBuilder
{

}

public enum DependencyLifestyle
{
    Transient,
    Scoped,
    Singleton,
}