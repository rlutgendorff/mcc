using Mcc.Ddd;
using Mcc.Repository.Databases;
using MongoDB.Driver;

namespace Mcc.Repository.Mongo;

public class MongoRepository<TEntity> : IDatabaseRepository<TEntity>
    where TEntity : IAggregate
{
    private readonly MongoContext _context;
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(MongoContext context, IEntityConfiguration<TEntity> configuration)
    {
        _context = context;
        _collection = context.GetCollection<TEntity>(configuration.TableName);
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return GetAll().Single(x => x.Id == id);
    }

    public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _collection.ReplaceOneAsync(
            c => c.Id == entity.Id,
            entity,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _collection.DeleteOneAsync(s => s.Id == entity.Id, cancellationToken);
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        var filter = Builders<TEntity>.Filter.In(d => d.Id, entities.Select(s => s.Id));

        await _collection.DeleteManyAsync(filter, cancellationToken);
    }

    public IDatabaseContextScope BeginScope()
    {
        return new MongoContextScope(_context);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _collection.AsQueryable();
    }
}
