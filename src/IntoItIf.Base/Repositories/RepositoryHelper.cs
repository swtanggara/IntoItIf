namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Options;
   using Exceptions;
   using Helpers;

   internal static class RepositoryHelper<T>
      where T : class
   {
      #region Methods

      internal static Option<Dictionary<string, object>> GetKeysAndValues(
         Option<string[]> keyProperties,
         Option<T> entity)
      {
         return entity.Combine(keyProperties)
            .Map(x => (Dictionary: x.Item1.ToDictionary(), KeyProperties: x.Item2))
            .Map(x => x.Dictionary.Where(y => x.KeyProperties.Contains(y.Key)).ToDictionary(y => y.Key, y => y.Value));
      }

      internal static Option<IPaged<TResult>> GetPagedBuiltByParameters<TResult>(
         Option<IQueryable<TResult>> sourceQuery,
         Option<IPageQuery> pageQuery,
         Option<string>[] defaultSortKeys)
         where TResult : class
      {
         return sourceQuery.ReduceOrDefault()
            .ToPaged(pageQuery.ReduceOrDefault(), defaultSortKeys.ReduceOrDefault().ToArray())
            .ToOption();
      }

      internal static async Task<Option<IPaged<TResult>>> GetPagedBuiltByParametersAsync<TResult>(
         Option<IQueryable<TResult>> sourceQuery,
         Option<Func<IQueryable<TResult>, CancellationToken, Task<List<TResult>>>> toListAsync,
         Option<IPageQuery> pageQuery,
         Option<string>[] defaultSortKeys,
         Option<CancellationToken> ctok)
         where TResult : class
      {
         return (await sourceQuery.ReduceOrDefault()
               .ToPagedAsync(
                  toListAsync.ReduceOrDefault(),
                  pageQuery.ReduceOrDefault(),
                  defaultSortKeys.ReduceOrDefault().ToArray(),
                  ctok.ReduceOrDefault()))
            .ToOption();
      }

      internal static Option<PageQuery> GetPageQueryMapping(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
      {
         return searchFields.Combine(pageIndex, true, PageQuery.DefaultIndexFrom.Id)
            .Combine(pageSize, true, PageQuery.DefaultPageSize)
            .Combine(sorts, true)
            .Combine(keyword, true)
            .Combine(indexFrom, true, PageQuery.DefaultIndexFrom)
            .MapFlatten(
               x => PageQuery.Get(
                  x.Item1.Item1.Item1.Item1.Item2,
                  x.Item1.Item1.Item1.Item2,
                  x.Item1.Item1.Item2,
                  x.Item1.Item2,
                  x.Item2,
                  x.Item1.Item1.Item1.Item1.Item1))
            .IfMapFlatten(
               x => x.SearchFields == null || !x.SearchFields.Any(),
               x => Fail<PageQuery>.Throw(new ArgumentNullException(nameof(searchFields))))
            .ElseMap(x => x)
            .Output;
      }

      internal static Fail<Dictionary<string, object>> ThrowCreateErrorExistingEntity(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated,
         Func<T, string[], string> defaultMessageFunc)
      {
         var result = Fail<Dictionary<string, object>>.Throw(
            validated.MessageFunc != null
               ? new ExistingEntityException<T>(validated.MessageFunc, validated.MatchValidatedEntity)
               : new ExistingEntityException<T>(
                  defaultMessageFunc,
                  validated.MatchValidatedEntity,
                  validated.PropertyNames));
         return result;
      }

      internal static Fail<Dictionary<string, object>> ThrowUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated,
         Func<T, string[], string> defaultMessageFunc)
      {
         Exception error = null;
         if (!validated.Found)
         {
            error = new KeyNotFoundException(
               defaultMessageFunc(validated.InputEntity, validated.PropertyNames));
         }
         else
         {
            if (validated.MatchValidatedEntity == null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x)))
            {
               if (validated.MessageFunc != null)
                  error = new ExistingEntityException<T>(validated.MessageFunc, validated.InputEntity);
               else
                  error = new ExistingEntityException<T>(
                     defaultMessageFunc,
                     validated.InputEntity,
                     validated.PropertyNames);
            }
         }

         return Fail<Dictionary<string, object>>.Throw(error);
      }

      internal static KeyValue ToKeyValue(T entity, string idProperty, string valueProperty, bool useValueAsId)
      {
         return useValueAsId
            ? new KeyValue(entity.GetPropertyValue(valueProperty), entity.GetPropertyValue(valueProperty))
            : new KeyValue(entity.GetPropertyValue(idProperty), entity.GetPropertyValue(valueProperty));
      }

      #endregion
   }
}