using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mcc.Repository.Mongo;

public class MongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoOptions> options)
    {
        _client = new MongoClient(options.Value.ConnectionString);
        _database = _client.GetDatabase(options.Value.Database);
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string table)
    {
        return _database.GetCollection<TEntity>(table);
    }

    public IClientSessionHandle StartSession()
    {
        return _client.StartSession();
    }
}