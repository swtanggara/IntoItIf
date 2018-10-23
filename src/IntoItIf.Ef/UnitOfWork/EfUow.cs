namespace IntoItIf.Ef.UnitOfWork
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Base.Domain.Options;
   using Base.UnitOfWork;
   using DbContexts;
   using Helpers;
   using Repositories;

   public sealed class EfUow : Uow<ItsDbContext>, IStringCommandUow, ITransactionUow
   {
      #region Constructors and Destructors

      public EfUow(Option<ItsDbContext> dataContext) : base(dataContext)
      {
      }

      #endregion

      #region Properties


      #endregion

      #region Public Methods and Operators

      public Option<int> ExecuteStringCommand(Option<string> sql, params Option<object>[] parameters)
      {
         return DataContext.MapFlatten(x => x.ToOption().ExecuteSqlCommand(sql, parameters));
      }

      public Task<Option<int>> ExecuteStringCommandAsync(
         Option<CancellationToken> ctok,
         Option<string> sql,
         params Option<object>[] parameters)
      {
         return DataContext.MapFlattenAsync(x => x.ToOption().ExecuteSqlCommandAsync(ctok, sql, parameters));
      }

      public Option<List<T>> FromString<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class
      {
         return DataContext.MapFlatten(x => x.ToOption().FromSql<T>(sql, parameters));
      }

      public Task<Option<List<T>>> FromStringAsync<T>(Option<string> sql, params Option<object>[] parameters)
         where T : class
      {
         return DataContext.MapFlattenAsync(x => x.ToOption().FromSqlAsync<T>(sql, parameters));
      }

      public Task<Option<List<T>>> FromStringAsync<T>(
         Option<CancellationToken> ctok,
         Option<string> sql,
         Option<object>[] parameters)
         where T : class
      {
         return DataContext.MapFlattenAsync(x => x.ToOption().FromSqlAsync<T>(ctok, sql, parameters));
      }

      public IUowDbTransaction GetDbTransaction<TContext>()
         where TContext : class, IDataContext
      {
         return DataContext.MapFlatten(x => x.ToOption().GetUowDbTransaction()).ReduceOrDefault();
      }

      public Option<EfRepository<T>> GetRepository<T>()
         where T : class
      {
         return GetRepository<EfRepository<T>, T>();
      }

      public override Option<int> SaveChanges()
      {
         return DataContext.MapFlatten(x => x.ToOption().SaveChanges());
      }

      public override Task<Option<int>> SaveChangesAsync()
      {
         return DataContext.MapFlattenAsync(x => x.ToOption().SaveChangesAsync());
      }

      public override Task<Option<int>> SaveChangesAsync(Option<CancellationToken> ctok)
      {
         return DataContext.MapFlattenAsync(x => x.ToOption().SaveChangesAsync(ctok));
      }

      #endregion

      #region Methods

      protected override Option<TRepository> InitRepository<TRepository, T>()
      {
         var repository = new EfRepository<T>(DataContext.ReduceOrDefault());
         return repository as TRepository;
      }

      #endregion
   }
}