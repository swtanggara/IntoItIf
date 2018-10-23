namespace IntoItIf.MongoDb
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Base.Domain.Options;
   using Base.UnitOfWork;

   public sealed class MongoUow : Uow<MongoDataContext>, IStringCommandUow, ITransactionUow
   {
      public MongoUow(Option<MongoDataContext> dataContext) : base(dataContext)
      {
      }

      public override Option<int> SaveChanges()
      {
         throw new System.NotImplementedException();
      }

      public override Task<Option<int>> SaveChangesAsync()
      {
         return SaveChangesAsync(None.Value);
      }

      public override Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok)
      {
         throw new System.NotImplementedException();
      }

      public IUowDbTransaction GetDbTransaction<TContext>()
         where TContext : class, IDataContext
      {
         throw new System.NotImplementedException();
      }

      public Option<MongoRepository<T>> GetRepository<T>()
         where T : class
      {
         return GetRepository<MongoRepository<T>, T>();
      }

      protected override Option<TRepository> InitRepository<TRepository, T>()
      {
         throw new System.NotImplementedException();
      }

      public Option<int> ExecuteStringCommand(Option<string> sql, params Option<object>[] parameters)
      {
         throw new System.NotImplementedException();
      }

      public Task<Option<int>> ExecuteStringCommandAsync(Option<CancellationToken> ctok, Option<string> sql, params Option<object>[] parameters)
      {
         throw new System.NotImplementedException();
      }

      public Option<List<T>> FromString<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class
      {
         throw new System.NotImplementedException();
      }

      public Task<Option<List<T>>> FromStringAsync<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class
      {
         throw new System.NotImplementedException();
      }

      public Task<Option<List<T>>> FromStringAsync<T>(Option<CancellationToken> ctok, Option<string> sql, Option<object>[] parameters)
         where T : class
      {
         throw new System.NotImplementedException();
      }
   }
}