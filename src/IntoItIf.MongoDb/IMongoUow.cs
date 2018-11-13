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

      Option<bool> RegisterChangesWatch<T>(Action<ChangeStreamDocument<T>> action, Option<ChangeStreamOperationType> operationType);
      Option<IMongoRepository<T>> SetOf<T>() where T : class;

      Option<TResult> SaveChangesForScoped<TResult>(
         Func<MongoUowScoped, Option<TResult>> func,
         Func<Exception, Option<TResult>> exceptionFunc);

      Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<Option<TResult>>> func,
         Func<Exception, Task<Option<TResult>>> exceptionFunc);

      Task<Option<TResult>> SaveChangesForScopedAsync<TResult>(
         Func<MongoUowScoped, Task<Option<TResult>>> func,
         Func<Exception, Task<Option<TResult>>> exceptionFunc,
         Option<CancellationToken> ctok);
   }
}