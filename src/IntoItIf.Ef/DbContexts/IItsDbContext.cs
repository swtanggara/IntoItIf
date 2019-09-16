#if NETSTANDARD2_0
namespace IntoItIf.Ef.DbContexts
{
   using System;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.ChangeTracking;
   using Microsoft.EntityFrameworkCore.Infrastructure;
   using Microsoft.EntityFrameworkCore.Metadata;

   public interface IItsDbContext<TContext> : IItsDbContext
      where TContext : DbContext
   {
   }

   public interface IItsDbContext : IRelationalDataContext
   {
      #region Public Properties

      DatabaseFacade Database { get; }
      ChangeTracker ChangeTracker { get; }
      IModel Model { get; }

      #endregion

      #region Public Methods and Operators

      EntityEntry<TEntity> Add<TEntity>(TEntity entity)
         where TEntity : class;

      EntityEntry Add(object entity);
      Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken);

      Task<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
         where TEntity : class;

      void AddRange(params object[] entities);
      void AddRange(IEnumerable<object> entities);
      Task AddRangeAsync(params object[] entities);
      Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken);

      EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
         where TEntity : class;

      EntityEntry Attach(object entity);
      void AttachRange(params object[] entities);
      void AttachRange(IEnumerable<object> entities);

      EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
         where TEntity : class;

      EntityEntry Entry(object entity);
      object Find(Type entityType, params object[] keyValues);

      TEntity Find<TEntity>(params object[] keyValues)
         where TEntity : class;

      Task<object> FindAsync(Type entityType, params object[] keyValues);
      Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);

      Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
         where TEntity : class;

      Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken)
         where TEntity : class;

      DbQuery<TQuery> Query<TQuery>()
         where TQuery : class;

      EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
         where TEntity : class;

      EntityEntry Remove(object entity);
      void RemoveRange(params object[] entities);
      void RemoveRange(IEnumerable<object> entities);

      int SaveChanges();
      int SaveChanges(bool acceptAllChangesOnSuccess);
      Task<int> SaveChangesAsync(CancellationToken cancellationToken);
      Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

      DbSet<TEntity> Set<TEntity>()
         where TEntity : class;

      EntityEntry<TEntity> Update<TEntity>(TEntity entity)
         where TEntity : class;

      EntityEntry Update(object entity);
      void UpdateRange(params object[] entities);
      void UpdateRange(IEnumerable<object> entities);

      #endregion
   }
}
#endif