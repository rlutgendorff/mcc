using Mcc.Ddd;
using Mcc.Di;
using Mcc.Repository.Databases;

namespace Mcc.Repository.Ef.SqlServer.Di;

public static class DependencyContainerBuilderExtensions
{
    private static void RegisterDatabaseRepository<TEntity>(this IDependencyContainerGenericBuilder builder)
        where TEntity : BaseAggregate
    {
        builder.Register<IDatabaseRepository<TEntity>, DbContextRepository<TEntity>>(DependencyLifestyle.Scoped);
        builder.Register<IRepository<TEntity>, DbContextRepository<TEntity>>(DependencyLifestyle.Scoped);

    }
}