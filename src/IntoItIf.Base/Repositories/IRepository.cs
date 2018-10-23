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

      Option<long> LongCount();
      Task<Option<long>> LongCountAsync();
      Option<Dictionary<string, object>> Create(Option<T> entity);
      Option<Dictionary<string, object>> Create(Option<T> entity, Option<Func<T, string>> existMessageFunc);
      Task<Option<Dictionary<string, object>>> CreateAsync(Option<T> entity);
      Task<Option<Dictionary<string, object>>> CreateAsync(Option<T> entity, Option<Func<T, string>> existMessageFunc);

      Task<Option<Dictionary<string, object>>> CreateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc,
         Option<CancellationToken> ctok);

      Option<bool> Delete(Option<T> entity);
      Task<Option<bool>> DeleteAsync(Option<T> entity);
      Task<Option<bool>> DeleteAsync(Option<T> entity, Option<CancellationToken> ctok);
      Option<T> GetFirstOrDefault(Option<Expression<Func<T, bool>>> predicate);

      Option<TResult> GetFirstOrDefault<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<T>> GetFirstOrDefaultAsync(Option<Expression<Func<T, bool>>> predicate);

      Task<Option<T>> GetFirstOrDefaultAsync(
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<TResult>> GetFirstOrDefaultAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok);

      Option<List<TResult>> GetList<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<List<TResult>>> GetListAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok);

      Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty);
      Option<List<KeyValue>> GetLookups(Option<string> idProperty, Option<string> valueProperty, Option<bool> useValueAsId);

      Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<Expression<Func<T, bool>>> predicate);

      Option<List<KeyValue>> GetLookups(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<List<KeyValue>>> GetLookupsAsync(Option<string> idProperty, Option<string> valueProperty);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate);

      Task<Option<List<KeyValue>>> GetLookupsAsync(
         Option<string> idProperty,
         Option<string> valueProperty,
         Option<bool> useValueAsId,
         Option<Expression<Func<T, bool>>> predicate,
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
         Option<Expression<Func<T, bool>>> predicate);

      Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<Expression<Func<T, bool>>> predicate)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom)
         where TResult : class;

      Option<IPaged<TResult>> GetPaged<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate)
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
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok);

      Task<Option<IPaged<TResult>>> GetPagedAsync<TResult>(
         Option<Expression<Func<T, TResult>>> selector,
         Option<string[]> searchFields,
         Option<int> pageIndex,
         Option<int> pageSize,
         Option<string[]> sorts,
         Option<string> keyword,
         Option<PageIndexFrom> indexFrom,
         Option<Expression<Func<T, bool>>> predicate,
         Option<CancellationToken> ctok)
         where TResult : class;

      Option<Dictionary<string, object>> Update(Option<T> entity);
      Option<Dictionary<string, object>> Update(Option<T> entity, Option<Func<T, string>> existMessageFunc);
      Task<Option<Dictionary<string, object>>> UpdateAsync(Option<T> entity);

      Task<Option<Dictionary<string, object>>> UpdateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc);

      Task<Option<Dictionary<string, object>>> UpdateAsync(
         Option<T> entity,
         Option<Func<T, string>> existMessageFunc,
         Option<CancellationToken> ctok);

      #endregion
   }
}