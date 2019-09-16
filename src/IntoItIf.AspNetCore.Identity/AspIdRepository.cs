namespace IntoItIf.AspNetCore.Identity
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Dynamic.Core;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.DataContexts;
   using Base.Domain;
   using Base.Domain.Options;
   using Base.Helpers;
   using Base.Repositories;
   using Ef.DbContexts;
   using Ef.Helpers;
   using LinqKit;

   public sealed class AspIdRepository<T> : BaseRelationalRepository<T>
      where T : class
   {
      #region Fields

      private readonly IItsDbContext _dataContext;

      #endregion

      #region Constructors and Destructors

      public AspIdRepository(IItsDbContext dataContext) : base(dataContext)
      {
         _dataContext = dataContext;
      }

      public AspIdRepository(IItsDbContext dataContext, IQueryable<T> customQuery) : base(dataContext, customQuery)
      {
      }

      #endregion

      #region Public Methods and Operators

      public override Dictionary<string, object> Add(T entity, Func<T, string> existMessageFunc)
      {
         return ProcessCreateAndGetResult(GetValidatedEntityForInsert(entity), existMessageFunc);
      }

      public override async Task<Dictionary<string, object>> AddAsync(T entity, Func<T, string> existMessageFunc, CancellationToken ctok)
      {
         return ProcessCreateAndGetResult(await GetValidatedEntityForInsertAsync(entity, ctok), existMessageFunc);
      }

      public override Dictionary<string, object> Change(T entity, Func<T, string> existMessageFunc)
      {
         return ProcessUpdateAndGetResult(
            GetValidatedEntityForUpdate(RelationalDataContext, GetBaseQuery(), entity),
            existMessageFunc);
      }

      public override async Task<Dictionary<string, object>> ChangeAsync(T entity, Func<T, string> existMessageFunc, CancellationToken ctok)
      {
         return ProcessUpdateAndGetResult(
            await GetValidatedEntityForUpdateAsync(RelationalDataContext, GetBaseQuery(), entity, ctok),
            existMessageFunc);
      }

      public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, bool disableTracking)
      {
         return GetFirstOrDefault(predicate: predicate, null, disableTracking: disableTracking);
      }

      public T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking)
      {
         return GetFirstOrDefault(predicate, orderBy, null, disableTracking);
      }

      public T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var x = GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking);
         return x.OrderBy != null ? x.OrderBy(x.Query).FirstOrDefault() : x.Query.FirstOrDefault();
      }

      public override T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetFirstOrDefault(predicate, orderBy, include, true);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
      {
         return GetFirstOrDefault(selector, predicate, null, disableTracking);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, null, disableTracking);
      }

      public TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var x = GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking);
         return x.OrderBy != null ? x.OrderBy(x.Query).Select(selector).FirstOrDefault() : x.Query.Select(selector).FirstOrDefault();
      }

      public override TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, include, true);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy: null, disableTracking: disableTracking, ctok: ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         var x = GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking);
         return x.OrderBy != null ? x.OrderBy(x.Query).ResolvedFirstOrDefaultAsync(ctok) : x.Query.ResolvedFirstOrDefaultAsync(ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, null, disableTracking, ctok);
      }

      public override Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, include, false, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, null, disableTracking, ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         var x = GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking);
         return x.OrderBy != null
            ? x.OrderBy(x.Query).Select(selector).ResolvedFirstOrDefaultAsync(ctok)
            : x.Query.Select(selector).ResolvedFirstOrDefaultAsync(ctok);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, null, disableTracking, ctok);
      }

      public override Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, include, true, ctok);
      }

      public List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking)
      {
         return GetList(selector, predicate, orderBy, null, disableTracking);
      }

      public List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         return orderBy != null ? orderBy(query).Select(selector).ToList() : query.Select(selector).ToList();
      }

      public override List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetList(selector, predicate, orderBy, include, true);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, orderBy, null, disableTracking, ctok);
      }

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         return orderBy != null
            ? orderBy(query).Select(selector).ResolvedToListAsync(ctok)
            : query.Select(selector).ResolvedToListAsync(ctok);
      }

      public override Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetListAsync(selector, predicate, orderBy, include, true, ctok);
      }

      public Option<List<KeyValue>> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         bool disableTracking)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, null, disableTracking);
      }

      public Option<List<KeyValue>> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, null, include, disableTracking);
      }

      public List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var orderByField = useValueAsId ? valueProperty : idProperty;
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         return query.OrderBy(orderByField)
            .Select(x => RepositoryHelper<T>.ToKeyValue(x, idProperty, valueProperty, useValueAsId))
            .ToList();
      }

      public override List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, include, true);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, null, disableTracking, ctok);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, null, include, disableTracking, ctok);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         var orderByField = useValueAsId ? valueProperty : idProperty;
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         return query.OrderBy(orderByField)
            .Select(x => RepositoryHelper<T>.ToKeyValue(x, idProperty, valueProperty, useValueAsId))
            .ResolvedToListAsync(ctok);
      }

      public override Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, include, false, ctok);
      }

      public override IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, true);
      }

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         bool disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, PageIndexFrom.Zero, null, null, disableTracking);
      }

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var pageQuery = RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom);
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         var sortKeys = RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext);
         return RepositoryHelper<T>.GetPagedBuiltByParameters(query, pageQuery, sortKeys);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            null,
            null,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            null,
            null,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            null,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            null,
            include,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            predicate,
            include,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            null,
            include,
            disableTracking);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            predicate,
            null,
            disableTracking);
      }

      public override IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, false);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         var pageQuery = RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom);
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         var sortKeys = RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext);
         return RepositoryHelper<T>.GetPagedBuiltByParameters(query, selector, pageQuery, sortKeys);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         bool disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            null,
            null,
            disableTracking,
            ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         bool disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            null,
            null,
            disableTracking,
            ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            null,
            disableTracking,
            ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            include,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            null,
            include,
            disableTracking,
            ctok);
      }

      public override Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         CancellationToken ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, true, ctok);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            include,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
      {
         var pageQuery = RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom);
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         var sortKeys = RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext);
         return RepositoryHelper<T>.GetPagedBuiltByParametersAsync(
            query,
            GetToListAsyncFunc<T>(),
            pageQuery,
            sortKeys,
            ctok);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         bool disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            null,
            null,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         bool disableTracking)
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
            null,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
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
            predicate,
            null,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
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
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            predicate,
            include,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            null,
            include,
            disableTracking,
            CancellationToken.None);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            PageIndexFrom.Zero,
            predicate,
            null,
            disableTracking,
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
         Func<IQueryable<T>, IQueryable<T>> include,
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
            include,
            false,
            ctok);
      }

      public Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking,
         CancellationToken ctok)
         where TResult : class
      {
         var pageQuery = RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom);
         var query = BuildQuery(GetBaseQuery(), predicate, include, disableTracking);
         var sortKeys = RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext);
         return RepositoryHelper<T>.GetPagedBuiltByParametersAsync(
            query,
            selector,
            GetToListAsyncFunc<TResult>(),
            pageQuery,
            sortKeys,
            ctok);
      }

      public override long LongCount(Expression<Func<T, bool>> predicate)
      {
         var query = GetBaseQuery();
         return predicate == null ? query.LongCount() : query.LongCount(predicate);
      }

      public override Task<long> LongCountAsync(
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
      {
         var query = GetBaseQuery();
         return predicate == null ? query.ResolvedLongCountAsync(ctok) : query.ResolvedLongCountAsync(predicate, ctok);
      }

      public override bool Remove(T entity)
      {
         return ProcessDeleteAndGetResult(GetValidatedEntityForDelete(entity));
      }

      public override async Task<bool> RemoveAsync(T entity, CancellationToken ctok)
      {
         return ProcessDeleteAndGetResult(await GetValidatedEntityForDeleteAsync(entity, ctok));
      }

      #endregion

      #region Methods

      protected override IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return BuildQuery(sourceQuery, predicate, include, false);
      }

      private bool AddEntry(T entity)
      {
         return _dataContext.AddEntry(entity);
      }

      private IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate,
         bool disableTracking)
      {
         return BuildQuery(sourceQuery, predicate, null, disableTracking);
      }

      private IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         bool disableTracking)
      {
         var isViewEntity = RelationalRepositoryHelper<T>.IsViewEntity();
         if (isViewEntity && !disableTracking) sourceQuery = sourceQuery.ResolvedAsNoTracking();
         if (!isViewEntity && include != null) sourceQuery = include(sourceQuery);
         return predicate == null ? sourceQuery : sourceQuery.AsExpandable().Where(predicate);
      }

      private Dictionary<string, object> CreateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         var success = AddEntry(validated.InputEntity);
         return success ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private Func<IQueryable<TResult>, CancellationToken, Task<List<TResult>>> GetToListAsyncFunc<TResult>()
      {
         return (query, token) => query.ResolvedToListAsync(token);
      }

      private (T Entity, T Exist) GetValidatedEntityForDelete(T entity)
      {
         var predicate = entity.BuildPredicate();
         var query = BuildQuery(GetBaseQuery(), predicate, true);
         return (entity, query.FirstOrDefault());
      }

      private async Task<(T Entity, T Exist)> GetValidatedEntityForDeleteAsync(T entity, CancellationToken ctok)
      {
         var predicate = entity.BuildPredicate();
         var query = BuildQuery(GetBaseQuery(), predicate, true);
         return (entity, await query.ResolvedFirstOrDefaultAsync(ctok));
      }

      private (T MatchValidatedEntity, string[] PropertyNames, T InputEntity) GetValidatedEntityForInsert(T entity)
      {
         Func<T, IQueryable<T>, (T ValidatedEntity, string[] PropertyNames, T InpuEntity)> getAkResult =
            (inEntity, inBaseQuery) =>
            {
               var akPredicate = RelationalDataContext.BuildAlternateKeyPredicate(inEntity);
               var akQuery = BuildQuery(inBaseQuery, akPredicate.Predicate, true);
               if (akQuery == null) return (null, akPredicate.PropertyNames, inEntity);
               var existByAk = akQuery.FirstOrDefault();
               return existByAk != null ? (existByAk, akPredicate.PropertyNames, inEntity) : (null, akPredicate.PropertyNames, inEntity);
            };
         var baseQuery = GetBaseQuery();
         var pkPredicate = RelationalDataContext.BuildPrimaryKeyPredicate(entity);
         var pkQuery = BuildQuery(GetBaseQuery(), pkPredicate.Predicate, true);
         if (pkQuery == null) return getAkResult(entity, baseQuery);
         var existByPk = pkQuery.FirstOrDefault();
         if (existByPk != null) return (existByPk, pkPredicate.PropertyNames, entity);
         return getAkResult(entity, baseQuery);
      }

      private async Task<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)> GetValidatedEntityForInsertAsync(
         T entity,
         CancellationToken ctok)
      {
         Func<T, IQueryable<T>, CancellationToken, Task<(T ValidatedEntity, string[] PropertyNames, T InpuEntity)>> getAkResultAsync =
            async (inEntity, inBaseQuery, inCtok) =>
            {
               var akPredicate = RelationalDataContext.BuildAlternateKeyPredicate(inEntity);
               var akQuery = BuildQuery(inBaseQuery, akPredicate.Predicate, true);
               if (akQuery == null) return (null, akPredicate.PropertyNames, inEntity);
               var existByAk = await akQuery.ResolvedFirstOrDefaultAsync(inCtok);
               return existByAk != null ? (existByAk, akPredicate.PropertyNames, inEntity) : (null, akPredicate.PropertyNames, inEntity);
            };
         var baseQuery = GetBaseQuery();
         var pkPredicate = RelationalDataContext.BuildPrimaryKeyPredicate(entity);
         var pkQuery = BuildQuery(GetBaseQuery(), pkPredicate.Predicate, true);
         if (pkQuery == null) return await getAkResultAsync(entity, baseQuery, ctok);
         var existByPk = await pkQuery.ResolvedFirstOrDefaultAsync(ctok);
         if (existByPk != null) return (existByPk, pkPredicate.PropertyNames, entity);
         return await getAkResultAsync(entity, baseQuery, ctok);
      }

      private (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity) GetValidatedEntityForUpdate(
         IRelationalDataContext dbContext,
         IQueryable<T> query,
         T entity)
      {
         var (pkPredicate, pkPropertyNames) = dbContext.BuildPrimaryKeyPredicate(entity);
         var (akPredicate, akPropertyNames) = dbContext.BuildAlternateKeyPredicate(entity);
         var keysProperties = RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(
            pkPropertyNames,
            akPropertyNames);
         var pkQuery = BuildQuery(query, pkPredicate, true);
         var akQuery = BuildQuery(query, akPredicate, true);
         var map = (
            ExistByPkEntity: pkQuery.FirstOrDefault(),
            ExistByAkEntity: akQuery.FirstOrDefault(),
            RealKeyPropertyNames: keysProperties,
            PkPropertyNames: pkPropertyNames,
            entity
         );
         return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(map);
      }

      private async Task<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)> GetValidatedEntityForUpdateAsync(
         IRelationalDataContext dbContext,
         IQueryable<T> query,
         T entity,
         CancellationToken ctok)
      {
         var primaryKeysPredicate = dbContext.BuildPrimaryKeyPredicate(entity);
         var alternateKeysPredicate = dbContext.BuildAlternateKeyPredicate(entity);
         var keysProperties = RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(
            primaryKeysPredicate.PropertyNames,
            alternateKeysPredicate.PropertyNames);
         var pkQuery = BuildQuery(query, primaryKeysPredicate.Predicate, true);
         var akQuery = BuildQuery(query, alternateKeysPredicate.Predicate, true);
         var map = (
            ExistByPkEntity: await pkQuery.ResolvedFirstOrDefaultAsync(ctok),
            ExistByAkEntity: await akQuery.ResolvedFirstOrDefaultAsync(ctok),
            RealKeyPropertyNames: keysProperties,
            PkPropertyNames: primaryKeysPredicate.PropertyNames,
            entity
         );
         return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(map);
      }

      private (IQueryable<T> Query, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy) GetValidatedFirstOrDefaultQuery(
         IQueryable<T> baseQuery,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         bool disableTracking)
      {
         return (BuildQuery(baseQuery, predicate, include, disableTracking), orderBy);
      }

      private Dictionary<string, object> ProcessCreateAndGetResult(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity) validated,
         Func<T, string> existMessageFunc)
      {
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.InputEntity, existMessageFunc);
         var createError = RelationalRepositoryHelper<T>.IfCreateError(mapped);
         if (createError) RepositoryHelper<T>.ThrowCreateErrorExistingEntity(mapped, DefaultExistMessageFunc);
         return CreateAndGetKeyValues(mapped);
      }

      private bool ProcessDeleteAndGetResult((T Entity, T Exist) validated)
      {
         if (validated.Exist != null) return RemoveEntry(validated.Exist);
         var props = PropertyHelper.GetProperties(validated.Entity)
            .Where(x => x.GetValue(validated.Entity) != null)
            .Select(x => x.Name)
            .ToArray();
         throw new KeyNotFoundException(DefaultNotFoundMessageFunc(validated.Entity, props));
      }

      private Dictionary<string, object> ProcessUpdateAndGetResult(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity) validated,
         Func<T, string> existMessageFunc)
      {
         var mapped = (validated.MatchValidatedEntity, validated.PropertyNames, validated.Found, validated.InputEntity, existMessageFunc);
         var updateError = RelationalRepositoryHelper<T>.IfUpdateError(mapped);
         if (updateError) RepositoryHelper<T>.ThrowUpdateError(mapped, DefaultExistMessageFunc);
         return UpdateAndGetKeyValues(mapped);
      }

      private bool RemoveEntry(T exist)
      {
         return _dataContext.RemoveEntry(exist);
      }

      private Dictionary<string, object> UpdateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         var updateSuccess = UpdateEntry(validated.InputEntity, validated.MatchValidatedEntity);
         return updateSuccess ? RepositoryHelper<T>.GetKeysAndValues(validated.PropertyNames, validated.InputEntity) : null;
      }

      private bool UpdateEntry(T entity, T exist)
      {
         return _dataContext.UpdateEntry(entity, exist);
      }

      #endregion
   }
}