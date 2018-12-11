namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.UnitOfWork;
   using MongoDB.Driver;

   public sealed class MongoUow : Uow<MongoDataContext>, IMongoUow
   {
      #region Constructors and Destructors

      public MongoUow(MongoDataContext dataContext) : base(dataContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public IUowDbTransaction GetDbTransaction()
      {
         return new MongoUowTransaction(DataContext.GetNewMongoSession());
      }

      public IMongoUowDbTransaction GetMongoDbTransaction()
      {
         return GetDbTransaction() as IMongoUowDbTransaction;
      }

      public Task RegisterChangesWatchAsync<T>(Action<ChangeStreamDocument<T>> action, ChangeStreamOperationType operationType)
      {
         return DataContext.RegisterChangesWatchAsync(operationType, action);
      }

      public TResult SaveChangesForScoped<TResult>(
         Func<MongoUowScoped, TResult> func,
         Func<Exception, TResult> exceptionFunc)
      {
         using (var session = GetDbTransaction())
         {
            var mongoSession = (MongoUowTransaction)session;
            try
            {
               var scoped = new MongoUowScoped(this, mongoSession.Transaction);
               var result = func(scoped);
               mongoSession.Commit();
               return result;
            }
            catch (Exception ex)
            {
               mongoSession.Rollback();
               return exceptionFunc(ex);
            }
         }
      }

      public Task<TResult> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<TResult>> func,
         Func<Exception, Task<TResult>> exceptionFunc)
      {
         return SaveChangesForScopedAsync(func, exceptionFunc, CancellationToken.None);
      }

      public async Task<TResult> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<TResult>> func,
         Func<Exception, Task<TResult>> exceptionFunc,
         CancellationToken ctok)
      {
         using (var session = GetDbTransaction())
         {
            var mongoSession = (MongoUowTransaction)session;
            try
            {
               var scoped = new MongoUowScoped(this, mongoSession.Transaction);
               var result = await func(scoped);
               await mongoSession.CommitAsync(ctok);
               return result;
            }
            catch (Exception ex)
            {
               await mongoSession.RollbackAsync(ctok);
               return await exceptionFunc(ex);
            }
         }
      }

      public IMongoRepository<T> SetOf<T>()
         where T : class
      {
         return GetRepository<MongoRepository<T>, T>();
      }

      #endregion

      #region Methods

      protected override TRepository InitRepository<TRepository, T>()
      {
         return new MongoRepository<T>(DataContext) as TRepository;
      }

      #endregion
   }
}