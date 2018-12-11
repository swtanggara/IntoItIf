namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using DataContexts;
   using Domain;

   public abstract class BaseRelationalRepository<T> : BaseRepository<T>
      where T : class
   {
      #region Constructors and Destructors

      protected BaseRelationalRepository(IRelationalDataContext dataContext) : this(dataContext, null)
      {
      }

      protected BaseRelationalRepository(IRelationalDataContext dataContext, IQueryable<T> customQuery) : base(
         dataContext,
         customQuery)
      {
         RelationalDataContext = dataContext;
      }

      #endregion

      #region Properties

      protected IRelationalDataContext RelationalDataContext { get; }

      #endregion

      #region Public Methods and Operators

      public override T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefault(predicate, null);
      }

      public T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetFirstOrDefault(predicate, orderBy, null);
      }

      public abstract T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include);

      public override TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefault(selector, predicate, null);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, null);
      }

      public abstract TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include);

      public override Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, null, ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, CancellationToken.None);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, null, ctok);
      }

      public abstract Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok);

      public override Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, null, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, null, CancellationToken.None);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, null, ctok);
      }

      public abstract Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok);

      public override List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetList(selector, predicate, null);
      }

      public List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetList(selector, predicate, orderBy, null);
      }

      public abstract List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include);

      public override Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, null, ctok);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
      {
         return GetListAsync(selector, predicate, orderBy, null, CancellationToken.None);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, orderBy, null, ctok);
      }

      public abstract Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok);

      public override List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, null);
      }

      public abstract List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include);

      public override Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, null, ctok);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, include, CancellationToken.None);
      }

      public abstract Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok);

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, null, include);
      }

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, include);
      }

      public override IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null);
      }

      public abstract IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include);

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, null, include);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, include);
      }

      public override IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null);
      }

      public abstract IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
         where TResult : class;

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, null, include, CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, null, include, ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, include, CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, include, ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null, CancellationToken.None);
      }

      public override Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null, ctok);
      }

      public abstract Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok);

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include)
         where TResult : class
      {
         return GetPagedAsync(selector, searchFields, pageIndex, pageSize, sorts, keyword, null, include);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            null,
            include,
            CancellationToken.None);
      }

      public override Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            null,
            ctok);
      }

      public abstract Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
         where TResult : class;

      public override long LongCount()
      {
         return LongCount(null);
      }

      public abstract long LongCount(Expression<Func<T, bool>> predicate);

      public Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
      {
         return LongCountAsync(predicate, CancellationToken.None);
      }

      public override Task<long> LongCountAsync(CancellationToken ctok)
      {
         return LongCountAsync(null, ctok);
      }

      public abstract Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken ctok);

      #endregion

      #region Methods

      protected override IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate)
      {
         return BuildQuery(sourceQuery, predicate, null);
      }

      protected abstract IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include);

      #endregion
   }
}