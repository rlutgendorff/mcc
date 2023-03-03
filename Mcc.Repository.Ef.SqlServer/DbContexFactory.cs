using Microsoft.EntityFrameworkCore;

namespace Mcc.Repository.Ef.SqlServer;

public class DbContexFactory<TEntity> : IDbContextFactory
    where TEntity : DbContext
{
    private readonly IDbContextFactory<TEntity> _dbContextFactory;

    public DbContexFactory(IDbContextFactory<TEntity> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public DbContext Create()
    {
        return _dbContextFactory.CreateDbContext();
    }
}