namespace _4EPlatForm.Infrastructure.Configurations
{
    public class GenricConfigration<T> : IEntityTypeConfiguration<T> where T : BaseEntity<Guid>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasQueryFilter(e => e.State == State.NotDeleted);

        }
    }
}
