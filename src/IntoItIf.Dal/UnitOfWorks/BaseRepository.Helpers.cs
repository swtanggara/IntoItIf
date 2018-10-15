namespace IntoItIf.Dal.UnitOfWorks
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Core.Domain;
   using Core.Domain.Entities;
   using Core.Domain.Options;
   using Core.Helpers;
   using DbContexts;
   using Exceptions;
   using Helpers;
   using LinqKit;

   public abstract partial class BaseRepository<T>
   {
      #region Methods

      protected static string DefaultNotFoundMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} ({dictEntity[propertyName]})")
            .ToList();
         var result =
            $"Cannot found entity with key(s) {string.Join(" and ", messageProperties)}.";
         return result;
      }

      protected IPaged<TResult> EfGetPagedBuiltByParameters<TResult>(
         (
            IQueryable<TResult> Query,
            IPageQuery PageQuery,
            string[] SortKeys) parameters)
         where TResult : class
      {
         return parameters.Query.ToPaged(parameters.PageQuery, parameters.SortKeys);
      }

      protected Task<IPaged<TResult>> EfGetPagedBuiltByParametersAsync<TResult>(
         (
            IQueryable<TResult> Query,
            IPageQuery PageQuery,
            string[] SortKeys,
            CancellationToken Ctok) parameters)
         where TResult : class
      {
         return parameters.Query.ToPagedAsync(parameters.PageQuery, parameters.SortKeys, parameters.Ctok);
      }

      protected IQueryable<T> EfGetQueryBuiltByParameters(
         (
            IQueryable<T> BaseQuery,
            Expression<Func<T, bool>> Predicate,
            Func<IQueryable<T>, IQueryable<T>> Include,
            bool DisableTracking,
            bool IsViewEntity) parameters)
      {
         if (!parameters.IsViewEntity && !parameters.DisableTracking)
            parameters.BaseQuery = parameters.BaseQuery.ResolvedAsNoTracking();
         if (!parameters.IsViewEntity && parameters.Include != null)
            parameters.BaseQuery = parameters.Include(parameters.BaseQuery);
         if (parameters.Predicate == null) return parameters.BaseQuery;
         parameters.BaseQuery = parameters.BaseQuery.AsExpandable();
         parameters.BaseQuery = parameters.BaseQuery.Where(parameters.Predicate);
         return parameters.BaseQuery;
      }

      private static Option<bool> AllKeysAreEqual(
         Option<T> existByPkEntity,
         Option<T> existByAkEntity,
         Option<string[]> pkPropertyNames)
      {
         return pkPropertyNames.Combine(existByPkEntity)
            .Combine(existByAkEntity)
            .Map(
               x =>
               {
                  var y = (
                     PkPropertyNames: x.Item1.Item1,
                     ExistByPkEntity: x.Item1.Item2,
                     ExistByAkEntity: x.Item2
                  );
                  foreach (var propertyName in y.PkPropertyNames)
                  {
                     var pkValue = y.ExistByPkEntity.GetPropertyValue(propertyName);
                     var akValue = y.ExistByAkEntity.GetPropertyValue(propertyName);
                     if (pkValue != akValue && (pkValue == null || !pkValue.Equals(akValue))) return false;
                  }

                  return true;
               });
      }

      private static string DefaultExistMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} ({dictEntity[propertyName]})")
            .ToList();
         var result =
            $"Cannot process entity with key(s) {string.Join(" and ", messageProperties)}. This key(s) already used " +
            "by another entity";
         return result;
      }

      private static Option<Dictionary<string, object>> GetKeysAndValues(
         Option<string[]> keyProperties,
         Option<T> entity)
      {
         return entity.Combine(keyProperties)
            .Map(x => (Dictionary: x.Item1.ToDictionary(), KeyProperties: x.Item2))
            .Map(x => x.Dictionary.Where(y => x.KeyProperties.Contains(y.Key)).ToDictionary(y => y.Key, y => y.Value));
      }

      private static Option<string[]> GetRealKeyPropertyNames(
         Option<string[]> pkPropertyNames,
         Option<string[]> akPropertyNames)
      {
         return pkPropertyNames.Combine(akPropertyNames)
            .Map(
               x =>
               {
                  var y = (PkPropertyNames: x.Item1, AkPropertyNames: x.Item2);
                  return y.AkPropertyNames == null || !y.AkPropertyNames.Any() ? y.PkPropertyNames : y.AkPropertyNames;
               });
      }

      private static Option<string[]> GetSortKeys(Option<IDbContext> dbContext)
      {
         return dbContext.Map(
               x => (DbContext: x, AkProperties: x.GetAlternateKeyProperties<T>().ReduceOrDefault()))
            .IfMap(
               x => x.AkProperties.Any(),
               x => x.AkProperties.Select(y => y.Name).ToArray())
            .ElseMap(
               x =>
               {
                  var pkProperties = x.DbContext.GetPrimaryKeyProperties<T>().ReduceOrDefault().ToArray();
                  return pkProperties.Any() ? pkProperties.Select(y => y.Name).ToArray() : null;
               })
            .Output;
      }

      private static Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)>
         GiveValidatedEntityForUpdateResult(
            Option<(
               T ExistByPkEntity,
               T ExistByAkEntity,
               string[] RealKeyPropertyNames,
               string[] PkPropertyNames,
               T InputEntity)> parms)
      {
         return parms.Map(
               y => (
                  y.ExistByPkEntity,
                  y.ExistByAkEntity,
                  y.RealKeyPropertyNames,
                  y.PkPropertyNames,
                  AllKeysAreEqual: AllKeysAreEqual(y.ExistByPkEntity, y.ExistByAkEntity, y.PkPropertyNames)
                     .ReduceOrDefault(),
                  y.InputEntity
               ))
            .IfMap(
               y => y.ExistByPkEntity == null && y.ExistByAkEntity == null,
               y => (
                  MatchValidatedEntity: (T)null,
                  PropertyNames: y.RealKeyPropertyNames,
                  Found: false,
                  y.InputEntity
               ))
            .ElseMap(
               y =>
               {
                  if (y.ExistByPkEntity == null || y.ExistByAkEntity != null && y.AllKeysAreEqual)
                     return (
                        MatchValidatedEntity: (T)null,
                        PropertyNames: y.RealKeyPropertyNames,
                        Found: true,
                        y.InputEntity
                     );

                  return (
                     MatchValidatedEntity: y.ExistByPkEntity,
                     PropertyNames: y.PkPropertyNames,
                     Found: true,
                     y.InputEntity
                  );
               })
            .Output;
      }

      private static bool IfCreateError(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         return validated.MatchValidatedEntity != null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
      }

      private static bool IfUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated)
      {
         if (!validated.Found) return true;
         return validated.MatchValidatedEntity == null &&
                validated.PropertyNames != null &&
                validated.PropertyNames.All(x => !string.IsNullOrWhiteSpace(x));
      }

      private static bool IsViewEntity()
      {
         return typeof(T).IsAssignableTo<IViewEntity>();
      }

      private static Fail<Dictionary<string, object>> ThrowCreateErrorExistingEntity(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated,
         Func<T, string[], string> defaultMessageFunc)
      {
         return Fail<Dictionary<string, object>>.Throw(
            validated.MessageFunc != null
               ? new ExistingEntityException<T>(validated.MessageFunc, validated.MatchValidatedEntity)
               : new ExistingEntityException<T>(
                  defaultMessageFunc,
                  validated.MatchValidatedEntity,
                  validated.PropertyNames));
      }

      private static Fail<Dictionary<string, object>> ThrowUpdateError(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated)
      {
         Exception error = null;
         if (!validated.Found)
         {
            error = new KeyNotFoundException(
               DefaultNotFoundMessageFunc(validated.InputEntity, validated.PropertyNames));
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
                     DefaultExistMessageFunc,
                     validated.InputEntity,
                     validated.PropertyNames);
            }
         }

         return Fail<Dictionary<string, object>>.Throw(error);
      }

      private static KeyValue ToIdValue(T entity, string idProperty, string valueProperty, bool useValueAsId)
      {
         return useValueAsId
            ? new KeyValue(entity.GetPropertyValue(valueProperty), entity.GetPropertyValue(valueProperty))
            : new KeyValue(entity.GetPropertyValue(idProperty), entity.GetPropertyValue(valueProperty));
      }

      private Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> query,
         Option<Expression<Func<T, bool>>> predicate)
      {
         return BuildQuery(query, predicate, None.Value);
      }

      private Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> query,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include)
      {
         return BuildQuery(query, predicate, include, false);
      }

      private Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> query,
         Option<Expression<Func<T, bool>>> predicate,
         Option<Func<IQueryable<T>, IQueryable<T>>> include,
         Option<bool> disableTracking)
      {
         return query
            .Combine(predicate, true)
            .Combine(include, true)
            .Combine(disableTracking, true, true)
            .Combine(IsViewEntity().ToOption())
            .Map(
               x => (
                  Query: x.Item1.Item1.Item1.Item1,
                  Predicate: x.Item1.Item1.Item1.Item2,
                  Include: x.Item1.Item1.Item2,
                  DisableTracking: x.Item1.Item2,
                  IsViewEntity: x.Item2
               ))
            .Map(GetQueryBuiltByParameters);
      }

      private Option<Dictionary<string, object>> CreateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, T InputEntity, Func<T, string> MessageFunc) validated)
      {
         return AddEntry(validated.InputEntity)
            .Map(y => (IsSuccess: y, validated.PropertyNames, validated.InputEntity))
            .IfMapFlatten(
               y => y.IsSuccess,
               y => GetKeysAndValues(y.PropertyNames, y.InputEntity))
            .Output;
      }

      private Option<PageQuery> GetPageQueryMapping(
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

      private Option<(T Entity, T Exist)> GetValidatedEntityForDelete(
         Option<T> entity,
         Option<IQueryable<T>> baseQuery)
      {
         return entity
            .Combine(baseQuery)
            .Map(x => (Entity: x.Item1, Predicate: x.Item1.BuildPredicate<T>(), BaseQuery: x.Item2))
            .Combine(x => BuildQuery(x.BaseQuery.ToOption(), x.Predicate))
            .Map(x => (x.Item1.Entity, Exist: x.Item2.FirstOrDefault()));
      }

      private Task<Option<(T Entity, T Exist)>> GetValidatedEntityForDeleteAsync(
         Option<T> entity,
         Option<IQueryable<T>> baseQuery,
         Option<CancellationToken> ctok)
      {
         return entity
            .Combine(baseQuery)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  Entity: x.Item1.Item1,
                  Predicate: x.Item1.Item1.BuildPredicate<T>(),
                  BaseQuery: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .Combine(x => BuildQuery(x.BaseQuery.ToOption(), x.Predicate))
            .Map(x => (x.Item1.Entity, Query: x.Item2, x.Item1.Ctok))
            .MapAsync(
               async x => (x.Entity, Exist: await x.Query.ResolvedFirstOrDefaultAsync(x.Ctok)));
      }

      private Option<(T MatchValidatedEntity, string[] PropertyNames, T InputEntity)>
         GetValidatedEntityForInsert(
            Option<IDbContext> dbContext,
            Option<IQueryable<T>> query,
            Option<T> entity)
      {
         return dbContext
            .Combine(query)
            .Combine(entity)
            .Map(x => (DbContext: x.Item1.Item1, Query: x.Item1.Item2, InputEntity: x.Item2))
            .MapFlatten(
               x =>
               {
                  return x.DbContext.BuildPrimaryKeyPredicate(x.InputEntity)
                     .Map(
                        y => (
                           ValidatedQuery: BuildQuery(x.Query.ToOption(), y.Predicate).ReduceOrDefault(),
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
                           return x.DbContext.BuildAlternateKeyPredicate(y.InputEntity)
                              .Map(
                                 z => (
                                    ValidatedQuery: BuildQuery(x.Query.ToOption(), z.Predicate).ReduceOrDefault(),
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
            Option<IDbContext> dbContext,
            Option<IQueryable<T>> query,
            Option<T> entity,
            Option<CancellationToken> ctok)
      {
         var result = dbContext
            .Combine(query)
            .Combine(entity)
            .Combine(ctok, true, CancellationToken.None)
            .Map(
               x => (
                  DbContext: x.Item1.Item1.Item1,
                  Query: x.Item1.Item1.Item2,
                  InputEntity: x.Item1.Item2,
                  Ctok: x.Item2
               ))
            .MapFlattenAsync(
               async x =>
               {
                  var _ = await x.DbContext.BuildPrimaryKeyPredicate(x.InputEntity)
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
                           var ___ = await x.DbContext.BuildAlternateKeyPredicate(x.InputEntity)
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
            Option<IDbContext> dbContext,
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
                        y => GetRealKeyPropertyNames(y.Item1.PropertyNames, y.Item2.PropertyNames))
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Predicate) /*Search by PK*/)
                     .Combine(y => BuildQuery(x.Query.ToOption(), y.Item1.Item1.Item2.Predicate) /*Search by AK*/)
                     .Map(
                        y => (
                           ExistByPkEntity: y.Item1.Item2.FirstOrDefault(),
                           ExistByAkEntity: y.Item2.FirstOrDefault(),
                           RealKeyPropertyNames: y.Item1.Item1.Item2,
                           PkPropertyNames: y.Item1.Item1.Item1.Item1.PropertyNames,
                           x.InputEntity
                        ));

                  return GiveValidatedEntityForUpdateResult(_);
               });
         return result;
      }

      private Task<Option<(T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity)>>
         GetValidatedEntityForUpdateAsync(
            Option<IDbContext> dbContext,
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
                        y => GetRealKeyPropertyNames(y.Item1.PropertyNames, y.Item2.PropertyNames))
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

                  return GiveValidatedEntityForUpdateResult(_);
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
            .IfMapFlatten(IfCreateError, x => ThrowCreateErrorExistingEntity(x, DefaultExistMessageFunc))
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
         return validated
            .Combine(existMessageFunc, true)
            .Map(
               x => (
                  x.Item1.MatchValidatedEntity,
                  x.Item1.PropertyNames,
                  x.Item1.Found,
                  x.Item1.InputEntity,
                  MessageFunc: x.Item2
               ))
            .IfMapFlatten(IfUpdateError, ThrowUpdateError)
            .ElseMapFlatten(UpdateAndGetKeyValues)
            .Output;
      }

      private Option<Dictionary<string, object>> UpdateAndGetKeyValues(
         (T MatchValidatedEntity, string[] PropertyNames, bool Found, T InputEntity, Func<T, string> MessageFunc)
            validated)
      {
         return UpdateEntry(validated.InputEntity, validated.MatchValidatedEntity)
            .Map(y => (IsSuccess: y, validated.PropertyNames, validated.InputEntity))
            .IfMapFlatten(
               y => y.IsSuccess,
               y => GetKeysAndValues(y.PropertyNames, y.InputEntity))
            .Output;
      }

      #endregion
   }
}