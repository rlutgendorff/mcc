using Microsoft.EntityFrameworkCore;

namespace Mcc.Repository.Ef.SqlServer;

public abstract class BaseDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions<TDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}