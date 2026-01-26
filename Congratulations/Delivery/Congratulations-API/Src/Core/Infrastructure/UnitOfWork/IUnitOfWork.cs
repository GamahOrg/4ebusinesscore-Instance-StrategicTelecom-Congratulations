



namespace _4EPlatForm.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGRepository<TEntity> Repository<TEntity>() where TEntity : AuditEntity<Guid>;
        IGRepository<TEntity> LK_Repository<TEntity>() where TEntity : AuditEntity<int>;


        #region Transaction
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        #endregion

        #region SaveChanges
        Task SaveChangesAsync();
        void SaveChanges();
        #endregion
    }
}
