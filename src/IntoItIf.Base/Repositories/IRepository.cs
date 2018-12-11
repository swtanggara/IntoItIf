namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;

   public interface IRepository<T>
   {
      #region Public Methods and Operators

      Dictionary<string, object> Add(T entity);
      Dictionary<string, object> Add(T entity, Func<T, string> existMessageFunc);
      Task<Dictionary<string, object>> AddAsync(T entity);
      Task<Dictionary<string, object>> AddAsync(T entity, Func<T, string> existMessageFunc);

      Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok);

      Dictionary<string, object> Change(T entity);
      Dictionary<string, object> Change(T entity, Func<T, string> existMessageFunc);
      Task<Dictionary<string, object>> ChangeAsync(T entity);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok);

      T GetFirstOrDefault(Expression<Func<T, bool>> predicate);

      TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

      Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ctok);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      List<TResult> GetList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<List<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      List<KeyValue> GetLookups(string idProperty, string valueProperty);
      List<KeyValue> GetLookups(string idProperty, string valueProperty, bool useValueAsId);

      List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         Expression<Func<T, bool>> predicate);

      List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate);

      Task<List<KeyValue>> GetLookupsAsync(string idProperty, string valueProperty);

      Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId);

      Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate);

      Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword);

      IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom);

      IPaged<T> GetPaged(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate);

      IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword)
         where TResult : class;

      IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom)
         where TResult : class;

      IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword);

      Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom);

      Task<IPaged<T>> GetPagedAsync(
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok);

      Task<IPaged<TResult>> GetPagedAsync<TResult>(
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

      long LongCount();
      Task<long> LongCountAsync();
      bool Remove(T entity);
      Task<bool> RemoveAsync(T entity);
      Task<bool> RemoveAsync(T entity, CancellationToken ctok);

      #endregion
   }
}