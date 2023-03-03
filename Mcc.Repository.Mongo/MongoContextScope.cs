using Mcc.Repository.Databases;
using MongoDB.Driver;

namespace Mcc.Repository.Mongo;

public class MongoContextScope : IDatabaseContextScope
{
    private readonly IClientSessionHandle _session;

    public MongoContextScope(MongoContext context)
    {
        _session = context.StartSession();
    }

    public void Dispose()
    {
        _session.CommitTransaction();
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(_session.CommitTransactionAsync());
    }
}