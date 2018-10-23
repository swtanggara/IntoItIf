namespace IntoItIf.Base.Helpers
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Linq.Dynamic.Core;
   using Humanizer;

   internal static class DynamicIQueryable
   {
      #region Methods

      internal static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string[] sorts)
      {
         if (sorts == null || !sorts.Any()) return query;
         var sortFields = new List<string>();
         foreach (var sort in sorts)
         {
            if (sort.StartsWith("+"))
               sortFields.Add($"{sort.TrimStart('+').Pascalize()} ASC");
            else if (sort.StartsWith("-"))
               sortFields.Add($"{sort.TrimStart('-').Pascalize()} DESC");
            else
               sortFields.Add(sort);
         }
         var result = query.OrderBy(string.Join(",", sortFields));
         return result;
      }

      #endregion
   }
}