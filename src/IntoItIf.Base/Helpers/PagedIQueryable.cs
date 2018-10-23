namespace IntoItIf.Base.Helpers
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Dynamic.Core;
   using System.Threading;
   using System.Threading.Tasks;
   using Domain;
   using Humanizer;

   internal static class PagedIQueryable
   {
      #region Methods

      internal static IQueryable<T> GetPerPageIQueryable<T>(
         this IQueryable<T> queryable,
         IPageQuery pageQuery,
         string[] defaultSortKeys,
         out int totalItems)
         where T : class
      {
         if (pageQuery == null) throw new ArgumentNullException(nameof(pageQuery));
         queryable = queryable.WhereByKeyword(
            pageQuery.Keyword,
            pageQuery.SearchFields.Pascalize());
         totalItems = queryable.Count();
         var pageSize = PageQuery.DefaultPageSize;
         var pageIndex = PageQuery.DefaultIndexFrom.Id;
         if (pageQuery.PageSize > 0) pageSize = pageQuery.PageSize;
         if (pageQuery.PageIndex > 0) pageIndex = pageQuery.PageIndex;
         if (pageQuery.Sorts != null && IsValidSorts<T>(pageQuery.Sorts))
            queryable = queryable.ApplySorting(pageQuery.Sorts);
         else
            queryable = queryable.OrderBy(string.Join(",", defaultSortKeys));

         var result = queryable.Skip((pageIndex - pageQuery.IndexFrom.Id) * pageSize)
            .Take(pageSize);
         return result;
      }

      internal static IPaged<T> ToPaged<T>(
         this IQueryable<T> queryable,
         IPageQuery pageQuery,
         string[] defaultSortKeys)
         where T : class
      {
         var pagedIQueryable = queryable.GetPerPageIQueryable(pageQuery, defaultSortKeys, out var totalItemsCount);
         var items = pagedIQueryable.ToList();
         return new Paged<T>(
            items,
            pageQuery.PageIndex,
            pageQuery.PageSize,
            pageQuery.IndexFrom,
            totalItemsCount);
      }

      internal static async Task<IPaged<T>> ToPagedAsync<T>(
         this IQueryable<T> queryable,
         Func<IQueryable<T>, CancellationToken, Task<List<T>>> toListAsync,
         IPageQuery pageQuery,
         string[] defaultSortKeys,
         CancellationToken ctok = default)
         where T : class
      {
         var pagedIQueryable = queryable.GetPerPageIQueryable(pageQuery, defaultSortKeys, out var totalItemsCount);
         var items = await toListAsync(pagedIQueryable, ctok);
         return new Paged<T>(
            items,
            pageQuery.PageIndex,
            pageQuery.PageSize,
            pageQuery.IndexFrom,
            totalItemsCount);
      }

      internal static IQueryable<T> WhereByKeyword<T>(
         this IQueryable<T> query,
         string keyword,
         params string[] fields)
         where T : class
      {
         var predicate = SearchInPropertyNames<T>.BuildPredicate(keyword, fields);
         return predicate == null ? query : query.Where(predicate);
      }

      internal static bool IsValidSorts<T>(string[] sorts)
         where T : class
      {
         if (sorts == null || !sorts.Any()) return false;
         var cleanedSorts = sorts
            .Select(
               x => x
                  .Replace("+", "")
                  .Replace("-", "")
                  .Trim()
                  .Pascalize())
            .ToArray();
         var validPropertyNames = cleanedSorts.GetValidatedPropertyNames<T>();
         return cleanedSorts.Length == validPropertyNames.Length;
      }

      #endregion
   }
}