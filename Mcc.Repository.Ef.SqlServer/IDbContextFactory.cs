using Microsoft.EntityFrameworkCore;

namespace Mcc.Repository.Ef.SqlServer;

public interface IDbContextFactory
{
    DbContext Create();
}