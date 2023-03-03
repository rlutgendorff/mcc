using Mcc.Repository.Databases;

namespace Mcc.Repository.Ef.SqlServer;

public class EfContextScope : IDatabaseContextScope
{
    private readonly Microsoft.EntityFrameworkCore.DbContext _dbContext;

    public EfContextScope(Microsoft.EntityFrameworkCore.DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Dispose()
    {
        _dbContext.SaveChanges();
    }

    public ValueTask DisposeAsync()
    {
         return new ValueTask(_dbContext.SaveChangesAsync());
    }
}