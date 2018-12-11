namespace IntoItIf.Ef.UnitOfWork
{
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain.Options;
   using Base.Repositories;
   using Base.UnitOfWork;
   using DbContexts;
   using Helpers;
   using Repositories;

   public sealed class EfUow : Uow<ItsDbContext>, IEfUow
   {
      #region Constructors and Destructors

      public EfUow(ItsDbContext dataContext) : base(dataContext)
      {
      }

      #endregion

      #region Properties


      #endregion

      #region Public Methods and Operators

      public int ExecuteStringCommand(string sql, params object[] parameters)
      {
         return DataContext.ExecuteSqlCommand(sql, parameters);
      }

      public Task<int> ExecuteStringCommandAsync(
         CancellationToken ctok,
         string sql,
         params object[] parameters)
      {
         return DataContext.ExecuteSqlCommandAsync(ctok, sql, parameters);
      }

      public List<T> FromString<T>(string sql, params object[] parameters)
         where T : class
      {
         return DataContext.FromSql<T>(sql, parameters);
      }

      public Task<List<T>> FromStringAsync<T>(string sql, params object[] parameters)
         where T : class
      {
         return DataContext.FromSqlAsync<T>(sql, parameters);
      }

      public Task<List<T>> FromStringAsync<T>(
         CancellationToken ctok,
         string sql,
         object[] parameters)
         where T : class
      {
         return DataContext.FromSqlAsync<T>(ctok, sql, parameters);
      }

      public IUowDbTransaction GetDbTransaction()
      {
         return DataContext.GetUowDbTransaction();
      }

      public Option<EfRepository<T>> GetRepository<T>()
         where T : class
      {
         return GetRepository<EfRepository<T>, T>();
      }

      public int SaveChanges()
      {
         return DataContext.SaveChanges();
      }

      public Task<int> SaveChangesAsync()
      {
         return DataContext.SaveChangesAsync();
      }

      public Task<int> SaveChangesAsync(CancellationToken ctok)
      {
         return DataContext.SaveChangesAsync(ctok);
      }

      public BaseRelationalRepository<T> SetOf<T>()
         where T : class
      {
         return GetRepository<EfRepository<T>, T>();
      }

      #endregion

      #region Methods

      protected override TRepository InitRepository<TRepository, T>()
      {
         var repository = new EfRepository<T>(DataContext);
         return repository as TRepository;
      }

      #endregion
   }
}