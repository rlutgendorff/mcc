using Mcc.Ddd;
using Mcc.Repository.Databases;
using Microsoft.EntityFrameworkCore;

namespace Mcc.Repository.Ef.SqlServer;

public class DbContextRepository<TEntity> : IDatabaseRepository<TEntity>
    where TEntity : BaseAggregate
{
    private readonly Microsoft.EntityFrameworkCore.DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public DbContextRepository(IDbContextFactory dbContextFactory)
    {
        _context = dbContextFactory.Create();
        _dbSet = _context.Set<TEntity>();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.Run(() => _dbSet.RemoveRange(entities), cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => _dbSet.Remove(entity), cancellationToken);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public Task<TEntity> GetByIdAsync(Guid id)
    {
        return _dbSet.SingleAsync(x => x.Id == id);
    }

    public Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return _dbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public IDatabaseContextScope BeginScope()
    {
        return new EfContextScope(_context);
    }
}