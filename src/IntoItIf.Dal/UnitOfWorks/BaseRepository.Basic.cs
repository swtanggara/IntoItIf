namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Options;
   using DbContexts;

   public abstract partial class BaseRepository<T>
      where T : class
   {
      #region Fields

      private readonly Option<IQueryable<T>> _customQuery;

      #endregion

      #region Constructors and Destructors

      protected BaseRepository(Option<IDbContext> dbContext) : this(dbContext, None.Value)
      {
      }

      protected BaseRepository(Option<IDbContext> dbContext, Option<IQueryable<T>> customQuery)
      {
         _customQuery = customQuery;
         DbContext = dbContext.ReduceOrDefault() ?? throw new ArgumentNullException(nameof(dbContext));
      }

      #endregion

      #region Properties

      internal IDbContext DbContext { get; }

      #endregion

      #region Public Methods and Operators

      public abstract Option<T> FindByKeys(params Option<object>[] keyValues);
      public abstract Option<Task<T>> FindByKeysAsync(params Option<object>[] keyValues);
      public abstract Option<Task<T>> FindByKeysAsync(Option<object>[] keyValues, Option<CancellationToken> ctok);

      #endregion

      #region Methods

      internal abstract IPaged<TResult> GetPagedBuiltByParameters<TResult>(
         (IQueryable<TResult> Query, IPageQuery PageQuery, string[] SortKeys) parameters)
         where TResult : class;

      internal abstract Task<IPaged<TResult>> GetPagedBuiltByParametersAsync<TResult>(
         (IQueryable<TResult> Query, IPageQuery PageQuery, string[] SortKeys, CancellationToken Ctok) parameters)
         where TResult : class;

      internal abstract IQueryable<T> GetQueryBuiltByParameters(
         (
            IQueryable<T> BaseQuery,
            Expression<Func<T, bool>> Predicate,
            Func<IQueryable<T>, IQueryable<T>> Include,
            bool DisableTracking,
            bool IsViewEntity) parameters);

      protected abstract Option<bool> AddEntry(Option<T> entity);
      protected abstract Option<bool> RemoveEntry(Option<T> exist);
      protected abstract Option<bool> UpdateEntry(Option<T> entity, Option<T> exist);

      private IQueryable<T> GetBaseQuery()
      {
         return (_customQuery.IsSome() ? _customQuery : DbContext.GetQuery<T>()).ReduceOrDefault();
      }

      #endregion
   }
}