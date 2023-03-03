namespace Mcc.Repository.Mongo;

public interface IEntityConfiguration<TEntity>
{
    string TableName { get; }
}