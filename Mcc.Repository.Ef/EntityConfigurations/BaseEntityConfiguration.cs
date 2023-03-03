using Mcc.Ddd;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mcc.Repository.Ef.EntityConfigurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IAggregate
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            if (IsTemporal)
                builder.ToTable(x => x.IsTemporal());
        }

        public abstract bool IsTemporal { get; }
    }
}
