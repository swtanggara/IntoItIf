namespace IntoItIf.MongoDb
{
   using System;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Options;
   using Base.UnitOfWork;
   using MongoDB.Driver;

   public interface IMongoUow : ITransactionUow
   {
      IMongoUowDbTransaction GetMongoDbTransaction();

      Task RegisterChangesWatchAsync<T>(Action<ChangeStreamDocument<T>> action, ChangeStreamOperationType operationType);

      IMongoRepository<T> SetOf<T>()
         where T : class;

      TResult SaveChangesForScoped<TResult>(
         Func<MongoUowScoped, TResult> func,
         Func<Exception, TResult> exceptionFunc);

      Task<TResult> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<TResult>> func,
         Func<Exception, Task<TResult>> exceptionFunc);

      Task<TResult> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<TResult>> func,
         Func<Exception, Task<TResult>> exceptionFunc,
         CancellationToken ctok);
   }
}