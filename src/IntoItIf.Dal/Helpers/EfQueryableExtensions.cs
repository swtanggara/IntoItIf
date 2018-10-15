#if NET471
namespace IntoItIf.Dal.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Data.Entity;
   using System.Linq;
   using System.Linq.Expressions;
   using System.Threading;
   using System.Threading.Tasks;
   using LinqKit;

   internal static class EfQueryableExtensions
   {
      #region Methods

      internal static Task<bool> ResolvedAllAsync<T>(
         IQueryable<T> query,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AllAsync(predicate, ctok);
      }

      internal static Task<bool> ResolvedAnyAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AnyAsync(ctok);
      }

      internal static Task<bool> ResolvedAnyAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AnyAsync(predicate, ctok);
      }

      internal static IQueryable<T> ResolvedAsNoTracking<T>(
         this IQueryable<T> query)
         where T : class
      {
         return query.AsNoTracking();
      }

      internal static Task<double> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, long>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<double> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, int>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<decimal> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, decimal>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<double> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, double>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<float> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, float>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<double?> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, long?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<double?> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, int?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<decimal?> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, decimal?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<double?> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, double?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<float?> ResolvedAverageAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, float?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.AverageAsync(selector, ctok);
      }

      internal static Task<bool> ResolvedContainsAsync<T>(
         this IQueryable<T> query,
         T item,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ContainsAsync(item, ctok);
      }

      internal static Task<int> ResolvedCountAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.CountAsync(ctok);
      }

      internal static Task<int> ResolvedCountAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok = default(CancellationToken))
      {
         query = query.AsExpandable();
         return query.CountAsync(predicate, ctok);
      }

      internal static Task<T> ResolvedFirstAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.FirstAsync(ctok);
      }

      internal static Task<T> ResolvedFirstAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.FirstAsync(predicate, ctok);
      }

      internal static Task<T> ResolvedFirstOrDefaultAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.FirstOrDefaultAsync(ctok);
      }

      internal static Task<T> ResolvedFirstOrDefaultAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, bool>> expression,
         CancellationToken ctok = default(CancellationToken))
      {
         query = query.AsExpandable();
         return query.FirstOrDefaultAsync(expression, ctok);
      }

      internal static Task ResolvedForEachAsync<T>(
         this IQueryable<T> query,
         Action<T> action,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ForEachAsync(action, ctok);
      }

      internal static IQueryable<T> ResolvedInclude<T, TProperty>(
         this IQueryable<T> query,
         Expression<Func<T, TProperty>> expression)
         where T : class
      {
         return query.Include(expression);
      }

      //internal static Task<T> ResolvedLastAsync<T>(
      //   this IQueryable<T> query,
      //   CancellationToken ctok = default(CancellationToken))
      //{
      //   return query.LastAsync(ctok);
      //}

      //internal static Task<T> ResolvedLastAsync<T>(
      //   this IQueryable<T> query,
      //   Expression<Func<T, bool>> predicate,
      //   CancellationToken ctok = default(CancellationToken))
      //{
      //   return query.LastAsync(predicate, ctok);
      //}

      internal static Task<long> ResolvedLongCountAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.LongCountAsync(ctok);
      }

      internal static Task<long> ResolvedLongCountAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, bool>> predicate,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.LongCountAsync(predicate, ctok);
      }

      internal static Task<T> ResolvedMaxAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.MaxAsync(ctok);
      }

      internal static Task<TResult> ResolvedMaxAsync<TSource, TResult>(
         this IQueryable<TSource> query,
         Expression<Func<TSource, TResult>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.MaxAsync(selector, ctok);
      }

      internal static Task<T> ResolvedMinAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.MinAsync(ctok);
      }

      internal static Task<TResult> ResolvedMinAsync<TSource, TResult>(
         this IQueryable<TSource> query,
         Expression<Func<TSource, TResult>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.MinAsync(selector, ctok);
      }

      internal static Task<long> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, long>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<int> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, int>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<decimal> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, decimal>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<double> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, double>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<float> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, float>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<long?> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, long?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<int?> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, int?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<decimal?> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, decimal?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<double?> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, double?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<float?> ResolvedSumAsync<T>(
         this IQueryable<T> query,
         Expression<Func<T, float?>> selector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.SumAsync(selector, ctok);
      }

      internal static Task<T[]> ResolvedToArrayAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ToArrayAsync(ctok);
      }

      internal static Task<Dictionary<TKey, T>> ResolvedToDictionaryAsync<T, TKey>(
         this IQueryable<T> query,
         Func<T, TKey> keySelector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ToDictionaryAsync(keySelector, ctok);
      }

      internal static Task<Dictionary<TKey, T>> ResolvedToDictionaryAsync<T, TKey>(
         this IQueryable<T> query,
         Func<T, TKey> keySelector,
         IEqualityComparer<TKey> comparer,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ToDictionaryAsync(keySelector, comparer, ctok);
      }

      internal static Task<Dictionary<TKey, TElement>> ResolvedToDictionaryAsync<T, TKey, TElement>(
         this IQueryable<T> query,
         Func<T, TKey> keySelector,
         Func<T, TElement> elementSelector,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ToDictionaryAsync(keySelector, elementSelector, ctok);
      }

      internal static Task<Dictionary<TKey, TElement>> ResolvedToDictionaryAsync<T, TKey, TElement>(
         this IQueryable<T> query,
         Func<T, TKey> keySelector,
         Func<T, TElement> elementSelector,
         IEqualityComparer<TKey> comparer,
         CancellationToken ctok = default(CancellationToken))
      {
         return query.ToDictionaryAsync(keySelector, elementSelector, comparer, ctok);
      }

      internal static Task<List<T>> ResolvedToListAsync<T>(
         this IQueryable<T> query,
         CancellationToken ctok = default(CancellationToken))
      {
         query = query.AsExpandable();
         return query.ToListAsync(ctok);
      }

      #endregion
   }
}
#endif