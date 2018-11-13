namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Options;
   using Base.UnitOfWork;
   using MongoDB.Driver;

   public sealed class MongoUow : Uow<MongoDataContext>, IMongoUow
   {
      #region Constructors and Destructors

      public MongoUow(Option<MongoDataContext> dataContext) : base(dataContext)
      {
      }

      #endregion

      #region Public Methods and Operators

      public IUowDbTransaction GetDbTransaction()
      {
         return DataContext
            .Map(x => new MongoUowTransaction(x.GetNewMongoSession().ReduceOrDefault()))
            .ReduceOrDefault();
      }

      public IMongoUowDbTransaction GetMongoDbTransaction()
      {
         return GetDbTransaction() as IMongoUowDbTransaction;
      }

      public Option<bool> RegisterChangesWatch<T>(Action<ChangeStreamDocument<T>> action, Option<ChangeStreamOperationType> operationType)
      {
         return DataContext.MapFlatten(x => x.RegisterChangesWatch(operationType, action));
      }

      public Option<TResult> SaveChangesForScoped<TResult>(
         Func<MongoUowScoped, Option<TResult>> func,
         Func<Exception, Option<TResult>> exceptionFunc)
      {
         return func.ToOption()
            .MapFlatten(
               x =>
               {
                  using (var session = GetDbTransaction())
                  {
                     var mongoSession = (MongoUowTransaction)session;
                     try
                     {
                        var scoped = new MongoUowScoped(this, mongoSession.Transaction.ToOption());
                        var result = x(scoped);
                        mongoSession.Commit();
                        return result;
                     }
                     catch (Exception ex)
                     {
                        mongoSession.Rollback();
                        return exceptionFunc(ex);
                     }
                  }
               });
      }

      public Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<Option<TResult>>> func,
         Func<Exception, Task<Option<TResult>>> exceptionFunc)
      {
         return SaveChangesForScopedAsync(func, exceptionFunc, None.Value);
      }

      public Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<Option<TResult>>> func,
         Func<Exception, Task<Option<TResult>>> exceptionFunc,
         Option<CancellationToken> ctok)
      {
         return func.ToOption()
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Func: x.Item1, Ctok: x.Item2))
            .MapFlattenAsync(
               async x =>
               {
                  using (var session = GetDbTransaction())
                  {
                     var mongoSession = (MongoUowTransaction)session;
                     try
                     {
                        var scoped = new MongoUowScoped(this, mongoSession.Transaction.ToOption());
                        var result = await x.Func(scoped);
                        await mongoSession.CommitAsync(x.Ctok);
                        return result;
                     }
                     catch (Exception ex)
                     {
                        await mongoSession.RollbackAsync(x.Ctok);
                        return await exceptionFunc(ex);
                     }
                  }
               });
      }

      public Option<IMongoRepository<T>> SetOf<T>()
         where T : class
      {
         return GetRepository<MongoRepository<T>, T>().ReduceOrDefault();
      }

      #endregion

      #region Methods

      protected override Option<TRepository> InitRepository<TRepository, T>()
      {
         return new MongoRepository<T>(DataContext) as TRepository;
      }

      #endregion
   }
}