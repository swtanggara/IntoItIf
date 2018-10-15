namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Dynamic.Core;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Options;
   using Helpers;

   public abstract partial class BaseRepository<T>
      where T : class
   {
      #region Public Methods and Operators

      public Option<int> Count()
      {
         return Count(None.Value);
      }

      public Option<int> Count(Option<Expression<Func<T, bool>>> predicate)
      {
         return predicate
            .IfMap(
               x => x == null,
               x => GetBaseQuery().Count())
            .ElseMap(
               x => GetBaseQuery().Count(x))
            .Output;
      }

      public Task<Option<int>> CountAsync()
      {
         return CountAsync(None.Value);
      }

      public Task<Option<int>> CountAsync(Option<Expression<Func<T, bool>>> predicate)
      {
         return CountAsync(predicate, None.Value);
      }

      public Task<Option<int>> CountAsync(Option<Expression<Func<T, bool>>> predicate, Option<CancellationToken> ctok)
      {
         return predicate.Combine(ctok, true, CancellationToken.None)
            .MapAsync(x => GetBaseQuery().ResolvedCountAsync(x.Item1, x.Item2));
      }

      public Option<Dictionary<string, object>> Create(Option<T> entity)
      {
         return Create(entity, None.Value);
      }

      public Option<Dictionary<string, object>> Create(Option<T> entity, Option<Func<T, string>> existMessageFunc)
      {
         return ProcessCreateAndGetResult(
            GetValidatedEntityForInsert(DbContext.ToOption(), GetBaseQuery().ToOption(), entity),
            existMessageFunc);
      }

      public Task<Option<Dictionary<string, object>>> CreateAsync(Option<T> entity)
      {
         return CreateAsync(entity, None.Value);
      }

      public Task<Option<Dictionary<string, object>>> CreateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc)
      {
         return CreateAsync(entity, existMessageFunc, None.Value);
      }

      public async Task<Option<Dictionary<string, object>>> CreateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc,
         Option<CancellationToken> ctok)
      {
         return ProcessCreateAndGetResult(
            await GetValidatedEntityForInsertAsync(DbContext.ToOption(), GetBaseQuery().ToOption(), entity, ctok),
            existMessageFunc);
      }

      public Option<bool> Delete(Option<T> entity)
      {
         return ProcessDeleteAndGetResult(GetValidatedEntityForDelete(entity, GetBaseQuery().ToOption()));
      }

      public Task<Option<bool>> DeleteAsync(Option<T> entity)
      {
         return DeleteAsync(entity, None.Value);
      }

      public async Task<Option<bool>> DeleteAsync(Option<T> entity, Option<CancellationToken> ctok)
      {
         return ProcessDeleteAndGetResult(
            await GetValidatedEntityForDeleteAsync(entity, GetBaseQuery().ToOption(), ctok));
      }

      public Option<T> GetFirstOrDefault(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking)
      {
         var validatedQuery = GetValidatedFirstOrDefaultQuery(
            GetBaseQuery().ToOption(),
            predicate,
            include,
            orderBy,
            disableTracking);
         return validatedQuery.Map(
            x => x.OrderBy != null ? x.OrderBy(x.Query).FirstOrDefault() : x.Query.FirstOrDefault());
      }

      public Option<TResult> GetFirstOrDefault<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking)
      {
         var validatedQuery = selector.Combine(
               GetValidatedFirstOrDefaultQuery(
                  GetBaseQuery().ToOption(),
                  predicate,
                  include,
                  orderBy,
                  disableTracking))
            .Map(x => (Selector: x.Item1, x.Item2.Query, x.Item2.OrderBy));
         return validatedQuery.Map(
            x => x.OrderBy != null
               ? x.OrderBy(x.Query).Select(x.Selector).FirstOrDefault()
               : x.Query.Select(x.Selector).FirstOrDefault());
      }

      public async Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = GetValidatedFirstOrDefaultQuery(
               GetBaseQuery().ToOption(),
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

      public async Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = selector.Combine(
               GetValidatedFirstOrDefaultQuery(
                  GetBaseQuery().ToOption(),
                  predicate,
                  include,
                  orderBy,
                  disableTracking))
            .Combine(ctok, true, CancellationToken.None)
            .Map(x => (Selector: x.Item1.Item1, x.Item1.Item2.Query, x.Item1.Item2.OrderBy, Ctok: x.Item2));
         return await validatedQuery.MapAsync(
            async x => x.OrderBy != null
               ? await x.OrderBy(x.Query).Select(x.Selector).ResolvedFirstOrDefaultAsync(x.Ctok)
               : await x.Query.Select(x.Selector).ResolvedFirstOrDefaultAsync(x.Ctok));
      }

      public Option<List<TResult>> GetList<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking)
      {
         var validatedQuery = selector
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(orderBy, true)
            .Map(x => (Selector: x.Item1.Item1, Query: x.Item1.Item2, OrderBy: x.Item2));
         return validatedQuery.Map(
            x => x.OrderBy != null
               ? x.OrderBy(x.Query).Select(x.Selector).ToList()
               : x.Query.Select(x.Selector).ToList());
      }

      public async Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         var validatedQuery = selector
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(orderBy, true)
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
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
      {
         return idProperty
            .Combine(valueProperty)
            .Combine(useValueAsId, true, true)
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
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
                  var result = x.Query.OrderBy(x.OrderByField)
                     .Select(y => ToIdValue(y, x.IdProperty, x.ValueProperty, x.UseValueAsId))
                     .ToList();
                  return result;
               });
      }

      public async Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking,
         Option<CancellationToken> ctok)
      {
         return await idProperty
            .Combine(valueProperty)
            .Combine(useValueAsId, true, true)
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
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
                     .Select(y => ToIdValue(y, x.IdProperty, x.ValueProperty, x.UseValueAsId))
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
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
      {
         return GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom)
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(GetSortKeys(DbContext.ToOption()))
            .Map(x => (PageQuery: x.Item1.Item1, DataQuery: x.Item1.Item2, SortKeys: x.Item2))
            .Map(x => GetPagedBuiltByParameters((x.DataQuery, x.PageQuery, x.SortKeys)));
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
         where TResult : class
      {
         return selector.Combine(GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom))
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(GetSortKeys(DbContext.ToOption()))
            .Map(
               x => (Selector: x.Item1.Item1.Item1, PageQuery: x.Item1.Item1.Item2, DataQuery: x.Item1.Item2,
                  SortKeys: x.Item2))
            .Map(x => GetPagedBuiltByParameters((x.DataQuery.Select(x.Selector), x.PageQuery, x.SortKeys)));
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
         return GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom)
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(GetSortKeys(DbContext.ToOption()))
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  PageQuery: x.Item1.Item1.Item1,
                  DataQuery: x.Item1.Item1.Item2,
                  SortKeys: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapAsync(x => GetPagedBuiltByParametersAsync((x.DataQuery, x.PageQuery, x.SortKeys, x.Ctok)));
      }

      public Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
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
         where TResult : class
      {
         return selector.Combine(GetPageQueryMapping(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom))
            .Combine(BuildQuery(GetBaseQuery().ToOption(), predicate, include, disableTracking))
            .Combine(GetSortKeys(DbContext.ToOption()))
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  Selector: x.Item1.Item1.Item1.Item1,
                  PageQuery: x.Item1.Item1.Item1.Item2,
                  DataQuery: x.Item1.Item1.Item2,
                  SortKeys: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapAsync(
               x => GetPagedBuiltByParametersAsync((x.DataQuery.Select(x.Selector), x.PageQuery, x.SortKeys, x.Ctok)));
      }

      public Option<Dictionary<string, object>> Update(Option<T> entity)
      {
         return Update(entity, None.Value);
      }

      public Option<Dictionary<string, object>> Update(Option<T> entity, Option<Func<T, string>> existMessageFunc)
      {
         return ProcessUpdateAndGetResult(
            GetValidatedEntityForUpdate(DbContext.ToOption(), GetBaseQuery().ToOption(), entity),
            existMessageFunc);
      }

      public Task<Option<Dictionary<string, object>>> UpdateAsync(Option<T> entity)
      {
         return UpdateAsync(entity, None.Value);
      }

      public Task<Option<Dictionary<string, object>>> UpdateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc)
      {
         return UpdateAsync(entity, existMessageFunc, None.Value);
      }

      public async Task<Option<Dictionary<string, object>>> UpdateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc,
         Option<CancellationToken> ctok)
      {
         return ProcessUpdateAndGetResult(
            await GetValidatedEntityForUpdateAsync(DbContext.ToOption(), GetBaseQuery().ToOption(), entity, ctok),
            existMessageFunc);
      }

      #endregion
   }
}