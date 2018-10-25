namespace IntoItIf.Base.Repositories
{
   using System;
   using System.Collections.Generic;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Domain.Options;

   public interface IRepository<T>
      where T : class
   {
      #region Public Methods and Operators

      Option<Dictionary<string, object>> Add(Option<T> entity);
      Option<Dictionary<string, object>> Add(Option<T> entity, Func<T, string> existMessageFunc);
      Task<Option<Dictionary<string, object>>> AddAsync(Option<T> entity);
      Task<Option<Dictionary<string, object>>> AddAsync(Option<T> entity, Func<T, string> existMessageFunc);

      Task<Option<Dictionary<string, object>>> AddAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok);

      Option<Dictionary<string, object>> Change(Option<T> entity);
      Option<Dictionary<string, object>> Change(Option<T> entity, Func<T, string> existMessageFunc);
      Task<Option<Dictionary<string, object>>> ChangeAsync(Option<T> entity);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc);

      Task<Option<Dictionary<string, object>>> ChangeAsync(
         Option<T> entity,
         Func<T, string> existMessageFunc,
         Option<CancellationToken> ctok);

      Option<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);

      Option<TResult> GetFirstOrDefault<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate);

      Task<Option<T>> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

      Task<Option<T>> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      Option<List<TResult>> GetList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<Option<List<TResult>>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate);

      Task<Option<List<TResult>>> GetListAsync<TResult>(
         Expression<Func<T, TResult>> selector,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty);
      Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty, Option<bool> useValueAsId);

      Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Expression<Func<T, bool>> predicate);

      Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate);

      Task<Option<List<KeyValue>>> GetLookupsAsync(Option<string> idProperty, Option<string> valueProperty);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword);

      Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom);

      Option<IPaged<T>> GetPaged(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate);

      Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Expression<Func<T, TResult>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate)
         where TResult : class;

      Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword);

      Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom);

      Task<Option<IPaged<T>>> GetPagedAsync(
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Expression<Func<T, bool>> predicate,
         Option<CancellationToken> ctok);

      Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
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

      Option<long> LongCount();
      Task<Option<long>> LongCountAsync();

      Option<bool> Remove(Option<T> entity);
      Task<Option<bool>> RemoveAsync(Option<T> entity);
      Task<Option<bool>> RemoveAsync(Option<T> entity, Option<CancellationToken> ctok);

      #endregion
   }
}