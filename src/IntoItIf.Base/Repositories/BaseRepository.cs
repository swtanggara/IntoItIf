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
   using Helpers;

   public abstract class BaseRepository<T> : IRepository<T>
      where T : class
   {
      #region Fields

      private readonly Option<IQueryable<T>> _customQuery;

      #endregion

      #region Constructors and Destructors

      protected BaseRepository(Option<IDataContext> dataContext) : this(dataContext, None.Value)
      {
      }

      protected BaseRepository(Option<IDataContext> dataContext, Option<IQueryable<T>> customQuery)
      {
         _customQuery = customQuery;
         DataContext = dataContext;
      }

      #endregion

      #region Properties

      protected Option<IDataContext> DataContext { get; }

      #endregion

      #region Public Methods and Operators

      public Option<Dictionary<string, object>> Add(Option<T> entity)
      {
         return Add(entity, null);
      }

      public abstract Option<Dictionary<string, object>> Add(Option<T> entity, Func<T, string> existMessageFunc);

      public Task<Option<Dictionary<string, object>>> AddAsync(Option<T> entity)
      {
         return AddAsync(entity, null);
      }

      public Task<Option<Dictionary<string, object>>> AddAsync(Option<T> entity, Func<T, string> existMessageFunc)
      {
         return AddAsync(entity, existMessageFunc, None.Value);
      }

      public abstract Task<Option<Dictionary<string, object>>> AddAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok);

      public Option<Dictionary<string, object>> Change(Option<T> entity)
      {
         return Change(entity, null);
      }

      public abstract Option<Dictionary<string, object>> Change(Option<T> entity, Func<T, string> existMessageFunc);

      public Task<Option<Dictionary<string, object>>> ChangeAsync(Option<T> entity)
      {
         return ChangeAsync(entity, null);
      }

      public Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc)
      {
         return ChangeAsync(entity, existMessageFunc, None.Value);
      }

      public abstract Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok);

      public abstract Option<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);

      public abstract Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      public Task<Option<T>> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefaultAsync(predicate, None.Value);
      }

      public abstract Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      public Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefaultAsync(selector, predicate, None.Value);
      }

      public abstract Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      public abstract Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      public Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetListAsync(selector, predicate, None.Value);
      }

      public abstract Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      public Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty)
      {
         return GetLookups(idProperty, valueProperty, null, null);
      }

      public Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty, Option<bool> useValueAsId)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, null);
      }

      public Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookups(idProperty, valueProperty, null, predicate);
      }

      public abstract Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate);

      public Task<Option<List<KeyValue>>> GetLookupsAsync(Option<string> idProperty, Option<string> valueProperty)
      {
         return GetLookupsAsync(idProperty, valueProperty, None.Value);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, null);
      }

      public Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, None.Value);
      }

      public abstract Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, None.Value);
      }

      public Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null);
      }

      public abstract Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate);

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, None.Value, null);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, None.Value, predicate);
      }

      public Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null);
      }

      public abstract Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, None.Value);
      }

      public Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, None.Value);
      }

      public abstract Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      public abstract Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok)
         where TResult : class;

      public abstract Option<long> LongCount();

      public Task<Option<long>> LongCountAsync()
      {
         return LongCountAsync(None.Value);
      }

      public abstract Task<Option<long>> LongCountAsync(Option<CancellationToken> ctok);

      public abstract Option<bool> Remove(Option<T> entity);

      public Task<Option<bool>> RemoveAsync(Option<T> entity)
      {
         return RemoveAsync(entity, None.Value);
      }

      public abstract Task<Option<bool>> RemoveAsync(Option<T> entity, Option<CancellationToken> ctok);

      #endregion

      #region Methods

      protected abstract Option<IQueryable<T>> BuildQuery(
         Option<IQueryable<T>> sourceQuery,
         Option<Expression<Func<T, bool>>> predicate);

      protected virtual string DefaultExistMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} ({dictEntity[propertyName]})")
            .ToList();
         return $"Cannot process entity with key(s) {string.Join(" and ", messageProperties)}. This key(s) already used " +
                "by another entity";
      }

      protected virtual string DefaultNotFoundMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} ({dictEntity[propertyName]})")
            .ToList();
         return $"Cannot found entity with key(s) {string.Join(" and ", messageProperties)}.";
      }

      protected Option<IQueryable<T>> GetBaseQuery()
      {
         return _customQuery.IsSome() ? _customQuery : DataContext.ReduceOrDefault().GetQuery<T>();
      }

      #endregion
   }
}