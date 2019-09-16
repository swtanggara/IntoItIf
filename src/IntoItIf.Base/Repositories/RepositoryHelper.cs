namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Exceptions;
   using Helpers;

   public static class RepositoryHelper<T>
      where T : class
   {
      #region Public Methods and Operators

      public static Dictionary<string, object> GetKeysAndValues(
         string[] keyProperties,
         T entity)
      {
         return entity.ToDictionary().Where(x => keyProperties.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
      }

      public static IPaged<T> GetPagedBuiltByParameters(
         IQueryable<T> sourceQuery,
         IPageQuery pageQuery,
         string[] defaultSortKeys)
      {
         return sourceQuery
            .ToPaged(pageQuery, defaultSortKeys.ToArray());
      }

      public static IPaged<TResult> GetPagedBuiltByParameters<TResult>(
         IQueryable<T> sourceQuery,
         Expression<Func<T, TResult>> selector,
         IPageQuery pageQuery,
         string[] defaultSortKeys)
         where TResult : class
      {
         return sourceQuery
            .ToPaged(selector, pageQuery, defaultSortKeys.ToArray());
      }

      public static async Task<IPaged<T>> GetPagedBuiltByParametersAsync(
         IQueryable<T> sourceQuery,
         Func<IQueryable<T>, CancellationToken, Task<List<T>>> toListAsync,
         IPageQuery pageQuery,
         string[] defaultSortKeys,
         CancellationToken ctok)
      {
         return await sourceQuery.ToPagedAsync(toListAsync, pageQuery, defaultSortKeys.ToArray(), ctok);
      }

      public static async Task<IPaged<TResult>> GetPagedBuiltByParametersAsync<TResult>(
         IQueryable<T> sourceQuery,
         Expression<Func<T, TResult>> selector,
         Func<IQueryable<TResult>, CancellationToken, Task<List<TResult>>> toListAsync,
         IPageQuery pageQuery,
         string[] defaultSortKeys,
         CancellationToken ctok)
      {
         return await sourceQuery.ToPagedAsync(selector, toListAsync, pageQuery, defaultSortKeys.ToArray(), ctok);
      }

      public static PageQuery GetPageQueryMapping(
         string[] searchFields,
         int pageIndex = PageQuery.DefaultIndexFrom,
         int pageSize = PageQuery.DefaultPageSize,
         string[] sorts = null,
         string keyword = null,
         PageIndexFrom indexFrom = null)
      {
         if (searchFields == null || !searchFields.Any())
         {
            throw new ArgumentNullException(nameof(searchFields));
         }

         return PageQuery.Get(pageIndex, pageSize, sorts, keyword, indexFrom, searchFields);
      }

      public static void ThrowCreateErrorExistingEntity(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated,
         Func<T, string[], string> defaultMessageFunc)
      {
         var error = validated.MessageFunc != null
            ? new ExistingEntityException<T>(validated.MessageFunc, validated.MatchValidatedEntity)
            : new ExistingEntityException<T>(
               defaultMessageFunc,
               validated.MatchValidatedEntity,
               validated.PropertyNames);
         throw error;
      }

      public static void ThrowUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated,
         Func<T, string[], string> defaultMessageFunc)
      {
         Exception error = new InvalidOperationException();
         if (!validated.Found)
         {
            error = new KeyNotFoundException(
               defaultMessageFunc(validated.InputEntity, validated.PropertyNames));
         }
         else
         {
            if (validated.MatchValidatedEntity != null ||
                validated.PropertyNames == null ||
                validated.PropertyNames.Any(string.IsNullOrWhiteSpace)) throw error;
            if (validated.MessageFunc != null)
            {
               error = new ExistingEntityException<T>(validated.MessageFunc, validated.InputEntity);
            }
            else
            {
               error = new ExistingEntityException<T>(
                  defaultMessageFunc,
                  validated.InputEntity,
                  validated.PropertyNames);
            }
         }

         throw error;
      }

      #endregion

      #region Methods

      internal static KeyValue ToKeyValue(T entity, string idProperty, string valueProperty, bool useValueAsId)
      {
         return useValueAsId
            ? new KeyValue(entity.GetPropertyValue(valueProperty), entity.GetPropertyValue(valueProperty))
            : new KeyValue(entity.GetPropertyValue(idProperty), entity.GetPropertyValue(valueProperty));
      }

      #endregion
   }
}