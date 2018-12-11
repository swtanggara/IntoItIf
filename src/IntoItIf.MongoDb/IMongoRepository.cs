namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Repositories;
   using MongoDB.Driver;

   public interface IMongoRepository<T> : IRepository<T>
      where T : class
   {
      Dictionary<string, object> Add(T entity, Func<T, string> existMessageFunc, IClientSessionHandle session);

      Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session);

      Task<Dictionary<string, object>> AddAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok);

      Dictionary<string, object> Change(
         T entity,
         params Expression<Func<T, object>>[] fieldSelections);

      Dictionary<string, object> Change(
         T entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections);

      Dictionary<string, object> Change(
         T entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Dictionary<string, object> Change(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Dictionary<string, object>> ChangeAsync(
         T entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         CancellationToken ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      T GetFirstOrDefault(Expression<Func<T, bool>> predicate, IClientSessionHandle session);

      T GetFirstOrDefault(Expression<Func<T, bool>> predicate, (bool Ascending, Expression<Func<T, object>> SortBy)? sort);

      TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session);

      T GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session);

      TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort);

      TResult GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session);

      Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, IClientSessionHandle session);

      Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort);

      Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session);

      Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok);

      Task<T> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         CancellationToken ctok);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         (bool Ascending, Expression<Func<T, object>> SortBy)? sort,
         IClientSessionHandle session,
         CancellationToken ctok);

      List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy);

      List<TResult> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session);

      Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         CancellationToken ctok);

      Task<List<TResult>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session,
         CancellationToken ctok);

      List<KeyValue> GetLookups(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session);

      Task<List<KeyValue>> GetLookupsAsync(
         string idProperty,
         string valueProperty,
         bool useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok);

      IPaged<TResult> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session);

      Task<IPaged<TResult>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         string[] searchFields,
         int pageIndex,
         int pageSize,
         string[] sorts,
         string keyword,
         PageIndexFrom indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok);

      long LongCount(Expression<Func<T, bool>> predicate);
      long LongCount(Expression<Func<T, bool>> predicate, IClientSessionHandle session);
      Task<long> LongCountAsync(CancellationToken ctok);
      Task<long> LongCountAsync(Expression<Func<T, bool>> predicate);
      Task<long> LongCountAsync(Expression<Func<T, bool>> predicate, CancellationToken ctok);

      Task<long> LongCountAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         CancellationToken ctok);

      T GetFirstOrDefault(T entity);
      TResult GetFirstOrDefault<TResult>(T entity, Expression<Func<T, TResult>> selector);
      Task<T> GetFirstOrDefaultAsync(T entity);
      Task<T> GetFirstOrDefaultAsync(T entity, CancellationToken ctok);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(T entity, Expression<Func<T, TResult>> selector);

      Task<TResult> GetFirstOrDefaultAsync<TResult>(
         T entity,
         Expression<Func<T, TResult>> selector,
         CancellationToken ctok);

      bool Remove(T entity, IClientSessionHandle session);

      Task<bool> RemoveAsync(T entity, IClientSessionHandle session, CancellationToken ctok);
   }
}