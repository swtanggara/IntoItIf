namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using DbContexts;

   public abstract class BaseUnitOfWork : IDisposable, IInjectable
   {
      #region Fields

      private readonly Option<IDbContext> _dbContext;
      private bool _disposed;
      private Dictionary<Type, object> _repositories;

      #endregion

      #region Constructors and Destructors

      protected BaseUnitOfWork(Option<IDbContext> dbContext)
      {
         _dbContext = dbContext;
      }

      #endregion

      #region Public Methods and Operators

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      public abstract Option<int> ExecuteSqlCommand(Option<string> sql, params Option<object>[] parameters);

      public abstract Task<Option<int>> ExecuteSqlCommandAsync(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters);

      public abstract Option<List<T>> FromSql<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class;

      public abstract Task<Option<List<T>>> FromSqlAsync<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class;

      public abstract Task<Option<List<T>>> FromSqlAsync<T>(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
         where T : class;

      public abstract IUowDbTransaction GetDbTransaction<TContext>()
         where TContext : class, IDbContext;

      public Option<BaseRepository<T>> GetRepository<T>()
         where T : class, IEntity
      {
         if (_repositories == null) _repositories = new Dictionary<Type, object>();

         var type = typeof(T);
         if (!_repositories.ContainsKey(type))
         {
#if NETSTANDARD2_0
            _repositories[type] = new EfCoreRepository<T>(_dbContext);
#elif NET471
            _repositories[type] = new EfRepository<T>(_dbContext);
#endif
         }

         return (BaseRepository<T>)_repositories[type];
      }

      public abstract Option<int> SaveChanges();

      public abstract Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok);

      public abstract Task<Option<int>> SaveChangesAsync(params Option<BaseUnitOfWork>[] unitOfWorks);

      public abstract Task<Option<int>> SaveChangesAsync(
         Option<CancellationToken> ctok,
         params Option<BaseUnitOfWork>[] unitOfWorks);

      public abstract Task<Option<int>> SaveChangesAsync();

      #endregion

      #region Methods

      internal Option<IDbContext> GetDbContext()
      {
         return _dbContext;
      }

      internal Option<TContext> GetDbContext<TContext>()
         where TContext : class, IDbContext
      {
         return _dbContext as Option<TContext>;
      }

      private void Dispose(bool disposing)
      {
         if (!_disposed)
            if (disposing)
            {
               // clear repositories
               _repositories?.Clear();
               // dispose the db context.
               _dbContext.ReduceOrDefault().Dispose();
            }

         _disposed = true;
      }

      #endregion
   }
}