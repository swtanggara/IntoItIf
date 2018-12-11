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
   using Helpers;

   public abstract class BaseRepository<T> : IRepository<T>
      where T : class
   {
      #region Fields

      private readonly IQueryable<T> _customQuery;

      #endregion

      #region Constructors and Destructors

      protected BaseRepository(IDataContext dataContext) : this(dataContext, null)
      {
      }

      protected BaseRepository(IDataContext dataContext, IQueryable<T> customQuery)
      {
         _customQuery = customQuery;
         DataContext = dataContext;
      }

      #endregion

      #region Properties

      protected IDataContext DataContext { get; }

      #endregion

      #region Public Methods and Operators

      public Dictionary<string, object> Add(T entity)
      {
         return Add(entity, null);
      }

      public abstract Dictionary<string, object> Add(T entity, Func<T, string> existMessageFunc);

      public Task<Dictionary<string, object>> AddAsync(T entity)
      {
         return AddAsync(entity, null);
      }

      public Task<Dictionary<string, object>> AddAsync(T entity, Func<T, string> existMessageFunc)
      {
         return AddAsync(entity, existMessageFunc, CancellationToken.None);
      }

      public abstract Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok);

      public Dictionary<string, object> Change(T entity)
      {
         return Change(entity, null);
      }

      public abstract Dictionary<string, object> Change(T entity, Func<T, string> existMessageFunc);

      public Task<Dictionary<string, object>> ChangeAsync(T entity)
      {
         return ChangeAsync(entity, null);
      }

      public Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc)
      {
         return ChangeAsync(entity, existMessageFunc, CancellationToken.None);
      }

      public abstract Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok);

      public T GetFirstOrDefault(T entity)
      {
         var predicate = entity.BuildPredicate();
         return GetFirstOrDefault(predicate);
      }

      public TResult GetFirstOrDefault<TResult>(T entity, Expression<Func<T, TResult>> selector)
      {
         var predicate = entity.BuildPredicate();
         return GetFirstOrDefault(selector, predicate);
      }

      public abstract T GetFirstOrDefault(Expression<Func<T, bool>> predicate);

      public abstract TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      public Task<T> GetFirstOrDefaultAsync(T entity)
      {
         return GetFirstOrDefaultAsync(entity, CancellationToken.None);
      }

      public Task<T> GetFirstOrDefaultAsync(T entity, CancellationToken ctok)
      {
         var predicate = entity.BuildPredicate();
         return GetFirstOrDefaultAsync(predicate, ctok);
      }

      public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefaultAsync(predicate, CancellationToken.None);
      }

      public abstract Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetFirstOrDefaultAsync(selector, predicate, CancellationToken.None);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(T entity, Expression<Func<T, TResult>> selector)
      {
         return GetFirstOrDefaultAsync(entity, selector, CancellationToken.None);
      }

      public Task<TResult> GetFirstOrDefaultAsync<TResult>(
         T entity,
         Expression<Func<T, TResult>> selector,
         CancellationToken ctok)
      {
         var predicate = entity.BuildPredicate();
         return GetFirstOrDefaultAsync(selector, predicate, ctok);
      }

      public abstract Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      public abstract List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      public Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate)
      {
         return GetListAsync(selector, predicate, CancellationToken.None);
      }

      public abstract Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      public List<KeyValue> GetLookups(string idProperty, string valueProperty)
      {
         return GetLookups(idProperty, valueProperty, false, null);
      }

      public List<KeyValue> GetLookups(string idProperty, string valueProperty, bool useValueAsId)
      {
         return GetLookups(idProperty, valueProperty, useValueAsId, null);
      }

      public List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookups(idProperty, valueProperty, false, predicate);
      }

      public abstract List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate);

      public Task<List<KeyValue>> GetLookupsAsync(string idProperty, string valueProperty)
      {
         return GetLookupsAsync(idProperty, valueProperty, false);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, null);
      }

      public Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate)
      {
         return GetLookupsAsync(idProperty, valueProperty, useValueAsId, predicate, CancellationToken.None);
      }

      public abstract Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, null);
      }

      public IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom)
      {
         return GetPaged(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null);
      }

      public abstract IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate);

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, null, null);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, null, predicate);
      }

      public IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom)
         where TResult : class
      {
         return GetPaged(selector, searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null);
      }

      public abstract IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, null);
      }

      public Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom)
      {
         return GetPagedAsync(searchFields, pageIndex, pageSize, sorts, keyword, indexFrom, null, CancellationToken.None);
      }

      public abstract Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      public abstract Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok)
         where TResult : class;

      public abstract long LongCount();

      public Task<long> LongCountAsync()
      {
         return LongCountAsync(CancellationToken.None);
      }

      public abstract Task<long> LongCountAsync(CancellationToken ctok);

      public abstract bool Remove(T entity);

      public Task<bool> RemoveAsync(T entity)
      {
         return RemoveAsync(entity, CancellationToken.None);
      }

      public abstract Task<bool> RemoveAsync(T entity, CancellationToken ctok);

      #endregion

      #region Methods

      protected abstract IQueryable<T> BuildQuery(
         IQueryable<T> sourceQuery,
         Expression<Func<T, bool>> predicate);

      protected virtual string DefaultExistMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} '{dictEntity[propertyName]}'")
            .ToList();
         return $"Cannot process {typeof(T).Name} with key(s) {string.Join(", ", messageProperties)}. These key(s) already used " +
                "by another entity";
      }

      protected virtual string DefaultNotFoundMessageFunc(T entity, string[] propertyNames)
      {
         var dictEntity = entity.ToDictionary();
         var messageProperties = propertyNames.Select(propertyName => $"{propertyName} ({dictEntity[propertyName]})")
            .ToList();
         return $"Cannot found {typeof(T).Name} with key(s) {string.Join(" and ", messageProperties)}.";
      }

      protected IQueryable<T> GetBaseQuery()
      {
         return _customQuery ?? DataContext.GetQuery<T>();
      }

      #endregion
   }
}