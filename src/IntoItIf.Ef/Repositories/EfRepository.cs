namespace IntoItIf.Ef.Repositories
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
   using DbContexts;
   using Helpers;
   using LinqKit;

   public sealed class EfRepository<T> : BaseRelationalRepository<T>
      where T : class
   {
      #region Constructors and Destructors

      public EfRepository(Option<ItsDbContext> dataContext) : base(dataContext.ReduceOrDefault())
      {
         ItsDbContext = dataContext;
      }

      public EfRepository(Option<ItsDbContext> dataContext, Option<IQueryable<T>> customQuery) : base(
         dataContext.ReduceOrDefault(),
         customQuery)
      {
         ItsDbContext = dataContext;
      }

      #endregion

      #region Properties

      private Option<ItsDbContext> ItsDbContext { get; }

      #endregion

      #region Public Methods and Operators

      public override Option<Dictionary<string, object>> Add(Option<T> entity, Func<T, string> existMessageFunc)
      {
         return ProcessCreateAndGetResult(GetValidatedEntityForInsert(entity), existMessageFunc);
      }

      public override async Task<Option<Dictionary<string, object>>> AddAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok)
      {
         return ProcessCreateAndGetResult(await GetValidatedEntityForInsertAsync(entity, ctok), existMessageFunc);
      }

      public override Option<Dictionary<string, object>> Change(Option<T> entity, Func<T, string> existMessageFunc)
      {
         return ProcessUpdateAndGetResult(
            GetValidatedEntityForUpdate(RelationalDataContext, GetBaseQuery(), entity),
            existMessageFunc);
      }

      public override async Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok)
      {
         return ProcessUpdateAndGetResult(
            await GetValidatedEntityForUpdateAsync(RelationalDataContext, GetBaseQuery(), entity, ctok),
            existMessageFunc);
      }

      public Option<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, Option<bool> disableTracking)
      {
         return GetFirstOrDefault(predicate: predicate, null, disableTracking: disableTracking);
      }

      public Option<T> GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking)
      {
         return GetFirstOrDefault(predicate, orderBy, null, disableTracking);
      }

      public override Option<T> GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetFirstOrDefault(predicate, orderBy, include, None.Value);
      }

      public Option<T> GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         var validatedQuery = GetValidatedFirstOrDefaultQuery(
            GetBaseQuery(),
            predicate,
            include,
            orderBy,
            disableTracking);
         return validatedQuery.Map(
            x => x.OrderBy != null ? x.OrderBy(x.Query).FirstOrDefault() : x.Query.FirstOrDefault());
      }

      public Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
      {
         return GetFirstOrDefault(selector, predicate, null, disableTracking);
      }

      public Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, null, disableTracking);
      }

      public override Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetFirstOrDefault(selector, predicate, orderBy, include, null);
      }

      public Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         var validatedQuery = selector.ToOption()
            .Combine(GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking))
            .Map(x => (Selector: x.Item1, x.Item2.Query, x.Item2.OrderBy));
         return validatedQuery.Map(
            x => x.OrderBy != null
               ? x.OrderBy(x.Query).Select(x.Selector).FirstOrDefault()
               : x.Query.Select(x.Selector).FirstOrDefault());
      }

      public Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy: null, disableTracking: disableTracking, ctok: ctok);
      }

      public Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, null, disableTracking, ctok);
      }

      public override Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(predicate, orderBy, include, None.Value, ctok);
      }

      public async Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = GetValidatedFirstOrDefaultQuery(
               GetBaseQuery(),
               predicate,
               include,
               orderBy,
               disableTracking)
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (x.Item1.Query, x.Item1.OrderBy, Ctok: x.Item2));
         return await validatedQuery.MapAsync(
            async x => x.OrderBy != null
               ? await x.OrderBy(x.Query).ResolvedFirstOrDefaultAsync(x.Ctok)
               : await x.Query.ResolvedFirstOrDefaultAsync(x.Ctok));
      }

      public Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, null, disableTracking, ctok);
      }

      public Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, null, disableTracking, ctok);
      }

      public override Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<CancellationToken> ctok)
      {
         return GetFirstOrDefaultAsync(selector, predicate, orderBy, include, null, ctok);
      }

      public async Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = selector.ToOption()
            .Combine(GetValidatedFirstOrDefaultQuery(GetBaseQuery(), predicate, include, orderBy, disableTracking))
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Selector: x.Item1.Item1, x.Item1.Item2.Query, x.Item1.Item2.OrderBy, Ctok: x.Item2));
         return await validatedQuery.MapAsync(
            async x => x.OrderBy != null
               ? await x.OrderBy(x.Query).Select(x.Selector).ResolvedFirstOrDefaultAsync(x.Ctok)
               : await x.Query.Select(x.Selector).ResolvedFirstOrDefaultAsync(x.Ctok));
      }

      public Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking)
      {
         return GetList(selector, predicate, orderBy, null, disableTracking);
      }

      public override Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetList(selector, predicate, orderBy, include, null);
      }

      public Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         var validatedQuery = selector.ToOption()
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(orderBy.ToOption(), true)
            .Map(x => (Selector: x.Item1.Item1, Query: x.Item1.Item2, OrderBy: x.Item2));
         return validatedQuery.Map(
            x => x.OrderBy != null
               ? x.OrderBy(x.Query).Select(x.Selector).ToList()
               : x.Query.Select(x.Selector).ToList());
      }

      public Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetListAsync(selector, predicate, orderBy, null, disableTracking, ctok);
      }

      public override Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<CancellationToken> ctok)
      {
         return GetListAsync(selector, predicate, orderBy, include, null, ctok);
      }

      public async Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = selector.ToOption()
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(orderBy.ToOption(), true)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  Selector: x.Item1.Item1.Item1,
                  Query: x.Item1.Item1.Item2,
                  OrderBy: x.Item1.Item2,
                  Ctok: x.Item2
               ));
         return await validatedQuery.MapAsync(
            async x => x.OrderBy != null
               ? await x.OrderBy(x.Query).Select(x.Selector).ResolvedToListAsync(x.Ctok)
               : await x.Query.Select(x.Selector).ResolvedToListAsync(x.Ctok));
      }

      public Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<bool> disableTracking)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, include: null, disableTracking: disableTracking);
      }

      public Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, null, include, disableTracking);
      }

      public Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, null, disableTracking);
      }

      public override Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, predicate, include, None.Value);
      }

      public Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return idProperty
            .Combine(valueProperty)
            .Combine(useValueAsId, true)
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Map(
               x => (
                  IdProperty: x.Item1.Item1.Item1,
                  ValueProperty: x.Item1.Item1.Item2,
                  UseValueAsId: x.Item1.Item2,
                  OrderByField: x.Item1.Item2 ? x.Item1.Item1.Item2 : x.Item1.Item1.Item1,
                  Query: x.Item2
               ))
            .Map(
               x =>
               {
                  return x.Query.OrderBy(x.OrderByField)
                     .Select(y => RepositoryHelper<T>.ToKeyValue(y, x.IdProperty, x.ValueProperty, x.UseValueAsId))
                     .ToList();
               });
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, include: null, disableTracking: disableTracking, ctok: ctok);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, null, include, disableTracking, ctok);
      }

      public override Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<CancellationToken> ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, include, None.Value, ctok);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, null, disableTracking, ctok);
      }

      public async Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return await idProperty
            .Combine(valueProperty)
            .Combine(useValueAsId, true, true)
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  IdProperty: x.Item1.Item1.Item1.Item1,
                  ValueProperty: x.Item1.Item1.Item1.Item2,
                  UseValueAsId: x.Item1.Item1.Item2,
                  OrderByField: x.Item1.Item1.Item2 ? x.Item1.Item1.Item1.Item2 : x.Item1.Item1.Item1.Item1,
                  Query: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapAsync(
               async x =>
               {
                  var result = x.Query.OrderBy(x.OrderByField)
                     .Select(y => RepositoryHelper<T>.ToKeyValue(y, x.IdProperty, x.ValueProperty, x.UseValueAsId))
                     .ResolvedToListAsync(x.Ctok);
                  return await result;
               });
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, null, null, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, null, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, null, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, include, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, predicate, include, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, null, include, disableTracking);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value, predicate, disableTracking);
      }

      public override Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, None.Value);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
      {
         return RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom)
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext))
            .Map(x => (PageQuery: x.Item1.Item1, DataQuery: x.Item1.Item2, SortKeys: x.Item2))
            .MapFlatten(
               x => RepositoryHelper<T>.GetPagedBuiltByParameters(
                  x.DataQuery.ToOption(),
                  x.PageQuery,
                  x.SortKeys.ToArrayOfOptions()));
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            null,
            null,
            disableTracking);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<bool> disableTracking)
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

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
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

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
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

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            predicate,
            include,
            disableTracking);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            null,
            include,
            disableTracking);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPaged(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            predicate,
            null,
            disableTracking);
      }

      public override Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include)
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, None.Value);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return selector.ToOption()
            .Combine(RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom))
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext))
            .Map(
               x => (Selector: x.Item1.Item1.Item1, PageQuery: x.Item1.Item1.Item2, DataQuery: x.Item1.Item2,
                  SortKeys: x.Item2))
            .MapFlatten(
               x => RepositoryHelper<T>.GetPagedBuiltByParameters(
                  x.DataQuery.Select(x.Selector).ToOption(),
                  x.PageQuery,
                  x.SortKeys.ToArrayOfOptions()));
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<bool> disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            disableTracking,
            None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            None.Value,
            None.Value,
            disableTracking,
            ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<bool> disableTracking)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            disableTracking,
            None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            None.Value,
            None.Value,
            disableTracking,
            ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            predicate,
            None.Value,
            disableTracking,
            ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            indexFrom,
            None.Value,
            include,
            disableTracking,
            ctok);
      }

      public override Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<CancellationToken> ctok)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, predicate, include, None.Value, ctok);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom)
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext))
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  PageQuery: x.Item1.Item1.Item1,
                  DataQuery: x.Item1.Item1.Item2,
                  SortKeys: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapFlattenAsync(
               x => RepositoryHelper<T>.GetPagedBuiltByParametersAsync(
                  x.DataQuery.ToOption(),
                  GetToListAsyncFunc<T>(),
                  x.PageQuery,
                  x.SortKeys.ToArrayOfOptions(),
                  x.Ctok));
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            null,
            null,
            disableTracking,
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
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
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            predicate,
            include,
            disableTracking,
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            null,
            include,
            disableTracking,
            None.Value);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate,
         Option<bool> disableTracking)
         where TResult : class
      {
         return GetPagedAsync(
            selector,
            searchFields,
            pageIndex,
            pageSize,
            sorts,
            keyword,
            None.Value,
            predicate,
            null,
            disableTracking,
            None.Value);
      }

      public override Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
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
            include,
            None.Value,
            ctok);
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Func<IQueryable<T>, IQueryable<T>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
         where TResult : class
      {
         return selector.ToOption()
            .Combine(RepositoryHelper<T>.GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom))
            .Combine(BuildQuery(GetBaseQuery(), predicate, include, disableTracking))
            .Combine(RelationalRepositoryHelper<T>.GetSortKeys(RelationalDataContext))
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  Selector: x.Item1.Item1.Item1.Item1,
                  PageQuery: x.Item1.Item1.Item1.Item2,
                  DataQuery: x.Item1.Item1.Item2,
                  SortKeys: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapFlattenAsync(
               x => RepositoryHelper<T>.GetPagedBuiltByParametersAsync(
                  x.DataQuery.Select(x.Selector).ToOption(),
                  GetToListAsyncFunc<TResult>(),
                  x.PageQuery,
                  x.SortKeys.ToArrayOfOptions(),
                  x.Ctok));
      }

      public override Option<long> LongCount(Option<Expression<Func<T, bool>>> predicate)
      {
         return predicate
            .Combine(GetBaseQuery())
            .Map(x => (Predicate: x.Item1, BaseQuery: x.Item2))
            .IfMap(
               x => x.Predicate == null,
               x => x.BaseQuery.LongCount())
            .ElseMap(
               x => x.BaseQuery.LongCount(x.Predicate))
            .Output;
      }

      public override async Task<Option<long>> LongCountAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
      {
         return (await predicate
            .Combine(GetBaseQuery())
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Predicate: x.Item1.Item1, BaseQuery: x.Item1.Item2, Ctok: x.Item2))
            .IfMapAsync(
               x => x.Predicate == null,
               x => x.BaseQuery.ResolvedLongCountAsync(x.Ctok))
            .ElseMapAsync(
               x => x.BaseQuery.ResolvedLongCountAsync(x.Predicate, x.Ctok))).Output;
      }

      public override Option<bool> Remove(Option<T> entity)
      {
         return ProcessDeleteAndGetResult(GetValidatedEntityForDelete(entity));
      }

      public override async Task<Option<bool>> RemoveAsync(Option<T> entity, Option<CancellationToken> ctok)
      {
         return ProcessDeleteAndGetResult(await GetValidatedEntityForDeleteAsync(entity, ctok));
      }

      #endregion

      #region Methods

      protected override Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return BuildQuery(sourceQuery, predicate, include, None.Value);
      }

      private Option<bool> AddEntry(Option<T> entity)
      {
         return ItsDbContext.AddEntry(entity);
      }

      private Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate,
         Option<bool> disableTracking)
      {
         return BuildQuery(sourceQuery, predicate, None.Value, disableTracking);
      }

      private Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
      {
         return sourceQuery
            .Combine(predicate, true)
            .Combine(include, true)
            .Combine(disableTracking, true, true)
            .Combine(RelationalRepositoryHelper<T>.IsViewEntity())
            .Map(
               x => (
                  SourceQuery: x.Item1.Item1.Item1.Item1,
                  Predicate: x.Item1.Item1.Item1.Item2,
                  Include: x.Item1.Item1.Item2,
                  DisableTracking: x.Item1.Item2,
                  IsViewEntity: x.Item2
               ))
            .Map(
               x =>
               {
                  if (!x.IsViewEntity && !x.DisableTracking)
                     x.SourceQuery = x.SourceQuery.ResolvedAsNoTracking();
                  if (!x.IsViewEntity && x.Include != null)
                     x.SourceQuery = x.Include(x.SourceQuery);
                  if (x.Predicate == null) return x.SourceQuery;
                  x.SourceQuery = x.SourceQuery.AsExpandable().Where(x.Predicate);
                  return x.SourceQuery;
               });
      }

      private Option<Dictionary<string, object>> CreateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         return AddEntry(validated.InputEntity)
            .Map(y => (IsSuccess: y, validated.PropertyNames, validated.InputEntity))
            .IfMapFlatten(
               y => y.IsSuccess,
               y => RepositoryHelper<T>.GetKeysAndValues(y.PropertyNames, y.InputEntity))
            .Output;
      }

      private Option<Func<IQueryable<TResult>, CancellationToken, Task<List<TResult>>>> GetToListAsyncFunc<TResult>()
      {
         Func<IQueryable<TResult>, CancellationToken, Task<List<TResult>>> result = (query, token) =>
            query.ResolvedToListAsync(token);
         return result;
      }

      private Option<(T Entity, T Exist)> GetValidatedEntityForDelete(
         Option<T> entity)
      {
         return entity
            .Combine(GetBaseQuery())
            .Map(x => (Entity: x.Item1, Predicate: x.Item1.BuildPredicate<T>(), BaseQuery: x.Item2))
            .Combine(x => BuildQuery(x.BaseQuery.ToOption(), x.Predicate, true))
            .Map(x => (x.Item1.Entity, Exist: x.Item2.FirstOrDefault()));
      }

      private Task<Option<(T Entity, T Exist)>> GetValidatedEntityForDeleteAsync(
         Option<T> entity,
         Option<CancellationToken> ctok)
      {
         return entity
            .Combine(GetBaseQuery())
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  Entity: x.Item1.Item1,
                  Predicate: x.Item1.Item1.BuildPredicate<T>(),
                  BaseQuery: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .Combine(x => BuildQuery(x.BaseQuery.ToOption(), x.Predicate, true))
            .Map(x => (x.Item1.Entity, Query: x.Item2, x.Item1.Ctok))
            .MapAsync(
               async x => (x.Entity, Exist: await x.Query.ResolvedFirstOrDefaultAsync(x.Ctok)));
      }

      private Option<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)>
         GetValidatedEntityForInsert(
            Option<T> entity)
      {
         return RelationalDataContext
            .Combine(GetBaseQuery())
            .Combine(entity)
            .Map(x => (DataContext: x.Item1.Item1, Query: x.Item1.Item2, InputEntity: x.Item2))
            .MapFlatten(
               x =>
               {
                  return x.DataContext.BuildPrimaryKeyPredicate(x.InputEntity)
                     .Map(
                        y => (
                           ValidatedQuery: BuildQuery(x.Query.ToOption(), y.Predicate, true).ReduceOrDefault(),
                           y.PropertyNames,
                           x.InputEntity
                        ))
                     .Map(y => (Exist: y.ValidatedQuery.FirstOrDefault(), y.PropertyNames, y.InputEntity))
                     .IfMap(
                        y => y.Exist != null,
                        y => (MatchValidatedEntity: y.Exist, y.PropertyNames, y.InputEntity))
                     .ElseMapFlatten(
                        y =>
                        {
                           return x.DataContext.BuildAlternateKeyPredicate(y.InputEntity)
                              .Map(
                                 z => (
                                    ValidatedQuery: BuildQuery(x.Query.ToOption(), z.Predicate, true).ReduceOrDefault(),
                                    z.PropertyNames,
                                    y.InputEntity
                                 ))
                              .Map(z => (Exist: z.ValidatedQuery.FirstOrDefault(), z.PropertyNames, z.InputEntity))
                              .IfMap(
                                 z => z.Exist != null,
                                 z => (MatchValidatedEntity: z.Exist, z.PropertyNames, z.InputEntity))
                              .ElseMap(z => (MatchValidatedEntity: (T)null, z.PropertyNames, z.InputEntity))
                              .Output;
                        })
                     .Output;
               });
      }

      private async Task<Option<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)>>
         GetValidatedEntityForInsertAsync(
            Option<T> entity,
            Option<CancellationToken> ctok)
      {
         var result = RelationalDataContext
            .Combine(GetBaseQuery())
            .Combine(entity)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  DataContext: x.Item1.Item1.Item1,
                  Query: x.Item1.Item1.Item2,
                  InputEntity: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapFlattenAsync(
               async x =>
               {
                  var _ = await x.DataContext.BuildPrimaryKeyPredicate(x.InputEntity)
                     .Map(
                        y => (
                           ValidatedQuery: BuildQuery(x.Query.ToOption(), y.Predicate).ReduceOrDefault(),
                           y.PropertyNames,
                           x.InputEntity,
                           x.Ctok
                        ))
                     .MapAsync(
                        async y => (
                           Exist: await y.ValidatedQuery.ResolvedFirstOrDefaultAsync(y.Ctok),
                           y.PropertyNames,
                           y.InputEntity,
                           y.Ctok
                        ));

                  var __ = await _
                     .IfMap(
                        y => y.Exist != null,
                        y => (MatchValidatedEntity: y.Exist, y.PropertyNames, y.InputEntity, y.Ctok))
                     .ElseMapFlattenAsync(
                        async y =>
                        {
                           var ___ = await x.DataContext.BuildAlternateKeyPredicate(x.InputEntity)
                              .Map(
                                 z => (
                                    ValidatedQuery: BuildQuery(x.Query.ToOption(), z.Predicate).ReduceOrDefault(),
                                    z.PropertyNames,
                                    y.InputEntity,
                                    y.Ctok
                                 ))
                              .MapAsync(
                                 async z => (
                                    Exist: await z.ValidatedQuery.ResolvedFirstOrDefaultAsync(z.Ctok),
                                    z.PropertyNames,
                                    z.InputEntity,
                                    z.Ctok
                                 ));

                           return ___
                              .IfMap(
                                 z => z.Exist != null,
                                 z => (MatchValidatedEntity: z.Exist, z.PropertyNames, z.InputEntity, z.Ctok))
                              .ElseMap(z => (MatchValidatedEntity: (T)null, z.PropertyNames, z.InputEntity, z.Ctok))
                              .Output;
                        });
                  return __.Output;
               });
         return (await result).Map(x => (x.MatchValidatedEntity, x.PropertyNames, x.InputEntity));
      }

      private Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)>
         GetValidatedEntityForUpdate(
            Option<IRelationalDataContext> dbContext,
            Option<IQueryable<T>> query,
            Option<T> entity)
      {
         var result = dbContext
            .Combine(query)
            .Combine(entity)
            .Map(x => (DbContext: x.Item1.Item1, Query: x.Item1.Item2, InputEntity: x.Item2))
            .MapFlatten(
               x =>
               {
                  var _ = x.DbContext.BuildPrimaryKeyPredicate(x.InputEntity)
                     .Combine(x.DbContext.BuildAlternateKeyPredicate(x.InputEntity))
                     .Combine( /*Properties are PK or AK*/
                        y => RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(
                           y.Item1.PropertyNames,
                           y.Item2.PropertyNames))
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Predicate, true) /*Search by PK*/)
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Item2.Predicate, true) /*Search by AK*/)
                     .Map(
                        y => (
                           ExistByPkEntity: y.Item1.Item2.FirstOrDefault(),
                           ExistByAkEntity: y.Item2.FirstOrDefault(),
                           RealKeyPropertyNames: y.Item1.Item1.Item2,
                           PkPropertyNames: y.Item1.Item1.Item1.Item1.PropertyNames,
                           x.InputEntity
                        ));

                  return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(_);
               });
         return result;
      }

      private Task<Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)>>
         GetValidatedEntityForUpdateAsync(
            Option<IRelationalDataContext> dbContext,
            Option<IQueryable<T>> query,
            Option<T> entity,
            Option<CancellationToken> ctok)
      {
         var result = dbContext
            .Combine(query)
            .Combine(entity)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (DbContext: x.Item1.Item1.Item1, Query: x.Item1.Item1.Item2, InputEntity: x.Item1.Item2,
                  Ctok: x.Item2))
            .MapFlattenAsync(
               async x =>
               {
                  var _ = await x.DbContext.BuildPrimaryKeyPredicate(x.InputEntity)
                     .Combine(x.DbContext.BuildAlternateKeyPredicate(x.InputEntity))
                     .Combine( /*Properties are PK or AK*/
                        y => RelationalRepositoryHelper<T>.GetKeyPropertyNamesInBetween(
                           y.Item1.PropertyNames,
                           y.Item2.PropertyNames))
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Predicate) /*Search by PK*/)
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Item2.Predicate) /*Search by AK*/)
                     .MapAsync(
                        async y => (
                           ExistByPkEntity: await y.Item1.Item2.ResolvedFirstOrDefaultAsync(x.Ctok),
                           ExistByAkEntity: await y.Item2.ResolvedFirstOrDefaultAsync(x.Ctok),
                           RealKeyPropertyNames: y.Item1.Item1.Item2,
                           PkPropertyNames: y.Item1.Item1.Item1.Item1.PropertyNames,
                           x.InputEntity
                        ));

                  return RelationalRepositoryHelper<T>.GiveValidatedEntityForUpdateResult(_);
               });
         return result;
      }

      private Option<(IQueryable<T> Query, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy)>
         GetValidatedFirstOrDefaultQuery(
            Option<IQueryable<T>> baseQuery,
            Option<Expression<Func<T, bool>>> predicate,
            Option<Func<IQueryable<T>, IQueryable<T>>> include,
            Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
            Option<bool> disableTracking)
      {
         return BuildQuery(baseQuery, predicate, include, disableTracking)
            .Combine(orderBy, true)
            .Map(x => (Query: x.Item1, OrderBy: x.Item2));
      }

      private Option<Dictionary<string, object>> ProcessCreateAndGetResult(
         Option<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)> validated,
         Option<Func<T, string>> existMessageFunc)
      {
         return validated.Combine(existMessageFunc, true)
            .Map(
               x => (
                  x.Item1.MatchValidatedEntity,
                  x.Item1.PropertyNames,
                  x.Item1.InputEntity,
                  MessageFunc: x.Item2
               ))
            .IfMapFlatten(
               RelationalRepositoryHelper<T>.IfCreateError,
               x => RepositoryHelper<T>.ThrowCreateErrorExistingEntity(x, DefaultExistMessageFunc))
            .ElseMapFlatten(CreateAndGetKeyValues)
            .Output;
      }

      private Option<bool> ProcessDeleteAndGetResult(Option<(T Entity, T Exist)> validated)
      {
         return validated.IfMapFlatten(
               x => x.Exist == null,
               x =>
               {
                  var properties = PropertyHelper.GetProperties(x.Entity)
                     .Where(y => y.GetValue(x.Entity) != null)
                     .Select(y => y.Name)
                     .ToArray();
                  return Fail<bool>.Throw(new KeyNotFoundException(DefaultNotFoundMessageFunc(x.Entity, properties)));
               })
            .ElseMapFlatten(
               x => RemoveEntry(x.Exist))
            .Output;
      }

      private Option<Dictionary<string, object>> ProcessUpdateAndGetResult(
         Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)> validated,
         Option<Func<T, string>> existMessageFunc)
      {
         return validated.Combine(existMessageFunc, true)
            .Map(
               x => (
                  x.Item1.MatchValidatedEntity,
                  x.Item1.PropertyNames,
                  x.Item1.Found,
                  x.Item1.InputEntity,
                  MessageFunc: x.Item2
               ))
            .IfMapFlatten(
               RelationalRepositoryHelper<T>.IfUpdateError,
               x => RepositoryHelper<T>.ThrowUpdateError(x, DefaultExistMessageFunc))
            .ElseMapFlatten(UpdateAndGetKeyValues)
            .Output;
      }

      private Option<bool> RemoveEntry(Option<T> exist)
      {
         return ItsDbContext.RemoveEntry(exist);
      }

      private Option<Dictionary<string, object>> UpdateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated)
      {
         return UpdateEntry(validated.InputEntity, validated.MatchValidatedEntity)
            .Map(y => (IsSuccess: y, validated.PropertyNames, validated.InputEntity))
            .IfMapFlatten(
               y => y.IsSuccess,
               y => RepositoryHelper<T>.GetKeysAndValues(y.PropertyNames, y.InputEntity))
            .Output;
      }

      private Option<bool> UpdateEntry(Option<T> entity, Option<T> exist)
      {
         return ItsDbContext.UpdateEntry(entity, exist);
      }

      #endregion
   }
}