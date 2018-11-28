namespace IntoItIf.MongoDb
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Base.Domain;
   using Base.Domain.Options;
   using Base.Repositories;
   using MongoDB.Driver;

   public interface IMongoRepository<T> : IRepository<T>
      where T : class
   {
      Option<Dictionary<string, object>> Add(Option<T> entity, Func<T, string> existMessageFunc, IClientSessionHandle session);

      Task<Option<Dictionary<string, object>>> AddAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session);

      Task<Option<Dictionary<string, object>>> AddAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<Dictionary<string, object>> Change(
         Option<T> entity,
         params Expression<Func<T, object>>[] fieldSelections);

      Option<Dictionary<string, object>> Change(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections);

      Option<Dictionary<string, object>> Change(
         Option<T> entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Option<Dictionary<string, object>> Change(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Option<CancellationToken> ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         IClientSessionHandle session,
         Option<CancellationToken> ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         params Expression<Func<T, object>>[] fieldSelections);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         IClientSessionHandle session,
         Option<CancellationToken> ctok,
         params Expression<Func<T, object>>[] fieldSelections);

      Option<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, IClientSessionHandle session);

      Option<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort);

      Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session);

      Option<T> GetFirstOrDefault(
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         IClientSessionHandle session);

      Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort);

      Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         IClientSessionHandle session);

      Task<Option<T>> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, IClientSessionHandle session);

      Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort);

      Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         IClientSessionHandle session);

      Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Task<Option<T>> GetFirstOrDefaultAsync(
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<(bool Ascending, Expression<Func<T, object>> SortBy)> sort,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy);

      Option<List<TResult>> GetList<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session);

      Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         Option<CancellationToken> ctok);

      Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Func<IFindFluent<T, T>, IOrderedFindFluent<T, T>> sortBy,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
         IClientSessionHandle session);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session);

      Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<long> LongCount(Option<Expression<Func<T, bool>>> predicate);
      Option<long> LongCount(Option<Expression<Func<T, bool>>> predicate, IClientSessionHandle session);
      Task<Option<long>> LongCountAsync(Option<CancellationToken> ctok);
      Task<Option<long>> LongCountAsync(Option<Expression<Func<T, bool>>> predicate);
      Task<Option<long>> LongCountAsync(Option<Expression<Func<T, bool>>> predicate, Option<CancellationToken> ctok);

      Task<Option<long>> LongCountAsync(
         Option<Expression<Func<T, bool>>> predicate,
         IClientSessionHandle session,
         Option<CancellationToken> ctok);

      Option<T> GetFirstOrDefault(T entity);
      Option<TResult> GetFirstOrDefault<TResult>(T entity, Expression<Func<T, TResult>> selector);
      Task<Option<T>> GetFirstOrDefaultAsync(T entity);
      Task<Option<T>> GetFirstOrDefaultAsync(T entity, Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(T entity, Expression<Func<T, TResult>> selector);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         T entity,
         Expression<Func<T, TResult>> selector,
         Option<CancellationToken> ctok);

      Option<bool> Remove(Option<T> entity, IClientSessionHandle session);

      Task<Option<bool>> RemoveAsync(Option<T> entity, IClientSessionHandle session, Option<CancellationToken> ctok);
   }
}