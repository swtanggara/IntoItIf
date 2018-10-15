namespace IntoItIf.Core.Helpers
{
   using System;
   using System.Linq;
   using System.Linq.Expressions;

   internal static class SearchInPropertyNames<T>
      where T : class
   {
      #region Methods

      internal static Expression<Func<T, bool>> BuildPredicate(string keyword, params string[] propertyNames)
      {
         var result = BuildPredicateInternal(keyword, propertyNames);
         return result;
      }

      private static Expression<Func<T, bool>> BuildPredicateInternal(string keyword, params string[] propertyNames)
      {
         if (propertyNames == null || !propertyNames.Any()) throw new ArgumentNullException(nameof(propertyNames));

         keyword = (keyword ?? "").Trim();
         if (keyword == "") return null;
         var tType = typeof(T);
         var parmExpr = Expression.Parameter(tType, "x");
         Expression body = null;
         var validatedPropertyNames = propertyNames.GetValidatedPropertyNames<T>();
         foreach (var propertyName in validatedPropertyNames)
         {
            var propExpr = Expression.Property(parmExpr, propertyName);
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var toLowerExpr = Expression.Call(propExpr, toLowerMethod ?? throw new InvalidOperationException());
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var valueExpr = Expression.Constant(keyword.ToLower());
            if (body == null)
            {
               body = Expression.Call(toLowerExpr, containsMethod ?? throw new InvalidOperationException(), valueExpr);
            }
            else
            {
               var newBody = Expression.Call(
                  toLowerExpr,
                  containsMethod ?? throw new InvalidOperationException(),
                  valueExpr);
               body = Expression.OrElse(body, newBody);
            }
         }

         if (body == null) return null;
         var result = Expression.Lambda<Func<T, bool>>(body, parmExpr);
         return result;
      }

      #endregion
   }
}