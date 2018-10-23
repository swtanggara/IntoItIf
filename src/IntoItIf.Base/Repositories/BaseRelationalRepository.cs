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
   using Domain.Options;

   public abstract class BaseRelationalRepository<T> : BaseRepository<T>
      where T : class
   {
      #region Constructors and Destructors

      protected BaseRelationalRepository(Option<IRelationalDataContext> dataContext) : this(dataContext, None.Value)
      {
      }

      protected BaseRelationalRepository(Option<IRelationalDataContext> dataContext, Option<IQueryable<T>> customQuery) : base(
         (dataContext.ReduceOrDefault() as IDataContext).ToOption(),
         customQuery)
      {
         RelationalDataContext = dataContext;
      }

      #endregion

      #region Properties

      protected Option<IRelationalDataContext> RelationalDataContext { get; }

      #endregion

      #region Public Methods and Operators

      public override Option<long> LongCount()
      {
         return LongCount(None.Value);
      }

      public abstract Option<long> LongCount(Option<Expression<Func<T, bool>>> predicate);

      public Task<Option<long>> LongCountAsync(Option<Expression<Func<T, bool>>> predicate)
      {
         return LongCountAsync(predicate, None.Value);
      }

      public override Task<Option<long>> LongCountAsync(Option<CancellationToken> ctok)
      {
         return LongCountAsync(None.Value, ctok);
      }

      public abstract Task<Option<long>> LongCountAsync(Option<Expression<Func<T, bool>>> predicate, Option<CancellationToken> ctok);

      public override Option<T> GetFirstOrDefault(Option<Expression<Func<T, bool>>> predicate)
      {
         return GetFirstOrDefault(predicate, None.Value);
      }

      public Option<T> GetFirstOrDefault(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetFirstOrDefault(predicate, orderBy, None.Value);
      }

      public abstract Option<T> GetFirstOrDefault(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      public override Option<TResult> GetFirstOrDefault<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetFirstOrDefault(selector, predicate, None.Value);
      }

      public Option<TResult> GetFirstOrDefault<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, None.Value);
      }

      public abstract Option<TResult> GetFirstOrDefault<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      public override Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(predicate, None.Value, ctok);
      }

      public Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, None.Value);
      }

      public Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, None.Value, ctok);
      }

      public abstract Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok);

      public override Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, None.Value, ctok);
      }

      public Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, None.Value, None.Value);
      }

      public Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, None.Value, ctok);
      }

      public abstract Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok);

      public override Option<List<TResult>> GetList<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetList(selector, predicate, None.Value);
      }

      public Option<List<TResult>> GetList<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetList(selector, predicate, orderBy, None.Value);
      }

      public abstract Option<List<TResult>> GetList<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      public override Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return GetListAsync(selector, predicate, None.Value, ctok);
      }

      public Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy)
      {
         return GetListAsync(selector, predicate, orderBy, None.Value, None.Value);
      }

      public Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<CancellationToken> ctok)
      {
         return GetListAsync(selector, predicate, orderBy, None.Value, ctok);
      }

      public abstract Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok);

      public override Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, None.Value);
      }

      public abstract Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      public override Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, None.Value, ctok);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, include, None.Value);
      }

      public abstract Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok);

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, include);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, None.Value, include);
      }

      public override Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, None.Value);
      }

      public abstract Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, None.Value, include);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, None.Value, include);
      }

      public override Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, None.Value);
      }

      public abstract Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
         where TResult : class;

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, include, None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, include, ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, None.Value, include, None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, None.Value, include, ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, None.Value, None.Value);
      }

      public override Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, None.Value, ctok);
      }

      public abstract Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok);

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
         where TResult : class
      {
         return GetPagedAsync(selector, searchFields, pageIndex, pageSize, sorts, keyword, None.Value, include);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
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
            None.Value,
            include,
            None.Value);
      }

      public override Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
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
            None.Value,
            ctok);
      }

      public abstract Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<CancellationToken> ctok)
         where TResult : class;

      #endregion

      #region Methods

      protected override Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return BuildQuery(sourceQuery, predicate, None.Value);
      }

      protected abstract Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include);

      #endregion
   }
}