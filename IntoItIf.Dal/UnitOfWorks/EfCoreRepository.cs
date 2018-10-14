#if NETSTANDARD2_0
namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Helpers;
   using DbContexts;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.ChangeTracking;

   internal sealed class EfCoreRepository<T> : BaseRepository<T>
      where T : class
   {
      #region Fields

      private readonly DbSet<T> _dbSet;
      private readonly DbContext _realDbContext;

      #endregion

      #region Constructors and Destructors

      public EfCoreRepository(Option<IDbContext> dbContext) : this(dbContext, None.Value)
      {
      }

      public EfCoreRepository(Option<IDbContext> dbContext, Option<IQueryable<T>> customQuery) : base(
         dbContext,
         customQuery)
      {
         _realDbContext = DbContext as DbContext ??
                          throw new InvalidOperationException("Invalid Microsoft.EntityFrameworkCore.DbContext");
         if (typeof(T).IsAssignableTo<IViewEntity>() || typeof(T).IsAssignableTo<IEntity>())
            _dbSet = _realDbContext.Set<T>();
      }

      #endregion

      #region Public Methods and Operators

      public override Option<T> FindByKeys(params Option<object>[] keyValues)
      {
         return keyValues.ToOptionOfEnumerable().Map(x => _dbSet.Find(x.ToArray()));
      }

      public override Option<Task<T>> FindByKeysAsync(params Option<object>[] keyValues)
      {
         return keyValues.ToOptionOfEnumerable().Map(x => _dbSet.FindAsync(x.ToArray()));
      }

      public override Option<Task<T>> FindByKeysAsync(Option<object>[] keyValues, Option<CancellationToken> ctok)
      {
         return keyValues.ToOptionOfEnumerable()
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => _dbSet.FindAsync(x.Item1.ToArray(), x.Item2));
      }

      #endregion

      #region Methods

      internal override IPaged<TResult> GetPagedBuiltByParameters<TResult>(
         (IQueryable<TResult> Query, IPageQuery PageQuery, string[] SortKeys) parameters)
      {
         return EfGetPagedBuiltByParameters(parameters);
      }

      internal override Task<IPaged<TResult>> GetPagedBuiltByParametersAsync<TResult>(
         (IQueryable<TResult> Query, IPageQuery PageQuery, string[] SortKeys, CancellationToken Ctok) parameters)
      {
         return EfGetPagedBuiltByParametersAsync(parameters);
      }

      internal override IQueryable<T> GetQueryBuiltByParameters(
         (IQueryable<T> BaseQuery, Expression<Func<T, bool>> Predicate, Func<IQueryable<T>, IQueryable<T>> Include, bool
            DisableTracking, bool IsViewEntity) parameters)
      {
         return EfGetQueryBuiltByParameters(parameters);
      }

      protected override Option<bool> AddEntry(Option<T> entity)
      {
         try
         {
            var map = entity.Map(x => _dbSet.Add(x));
            if (!(map is Some<EntityEntry<T>>))
               return Fail<bool>.Throw(
                  new InvalidOperationException(
                     "Invalid processing result of Microsoft.EntityFrameworkCore.DbSet<T>.Add()"));
            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      protected override Option<bool> RemoveEntry(Option<T> exist)
      {
         try
         {
            var map = exist.Map(x => _dbSet.Remove(x));
            if (!(map is Some<EntityEntry<T>>))
               return Fail<bool>.Throw(
                  new InvalidOperationException(
                     "Invalid processing result of Microsoft.EntityFrameworkCore.DbSet<T>.Remove()"));

            return true;
         }
         catch (Exception ex)
         {
            return Fail<bool>.Throw(ex);
         }
      }

      protected override Option<bool> UpdateEntry(Option<T> entity, Option<T> exist)
      {
         return entity.Combine(exist)
            .Map(x => (Entity: x.Item1, Exist: x.Item2, DbContext: _realDbContext))
            .MapFlatten(
               x =>
               {
                  try
                  {
                     var entry = x.DbContext.Entry(x.Exist);
                     entry.CurrentValues.SetValues(x.Entity);
                     // ReSharper disable once SuspiciousTypeConversion.Global
                     if (!(x.Entity is IRowVersion rowVersionEntity)) return true.ToOption();
                     entry.OriginalValues["RowVersion"] = rowVersionEntity.RowVersion;
                     return true.ToOption();
                  }
                  catch (Exception ex)
                  {
                     return Fail<bool>.Throw(ex);
                  }
               });
      }

      #endregion
   }
}
#endif